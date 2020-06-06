using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceRegistry.Controllers;
using ServiceRegistry.Repositories;

namespace ServiceRegistry.Extensions
{
    public static class MonitorExtensions
    {
        private const int MaxFailures = 3;
        private static readonly int TimeoutInSeconds = 5;

        public static void UseInstancesMonitoring(this IApplicationBuilder app, ILogger logger)
        {
            Task.Run(async () =>
            {
                IDictionary<string, int> failures = new Dictionary<string, int>();
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(TimeoutInSeconds));
                    logger.LogInformation("Periodic service instances health check");

                    var repository =
                        (IInstanceRepository) app.ApplicationServices.GetService(typeof(IInstanceRepository));
                    foreach (var instance in repository.GetInstanceStates())
                    {
                        if (!instance.HasHealthCheck)
                        {
                            logger.LogInformation($"Instance {instance.InstanceId} doesn't have HC");
                            continue;
                        }

                        var isHealthy = await IsHealthy(instance, logger);
                        if (isHealthy)
                        {
                            logger.LogInformation($"Instance {instance.InstanceId} is healthy");
                            RemoveFromFailed(failures, instance);
                            continue;
                        }

                        logger.LogWarning($"Not healthy instance {JsonConvert.SerializeObject(instance)}");
                        IncreaseFailures(failures, instance);
                        RemoveInstanceOnMaxFailure(failures, instance, repository, logger);
                    }
                }
            });
        }

        private static async Task<bool> IsHealthy(ServiceInstanceData instance, ILogger logger)
        {
            try
            {
                var httpClient = new HttpClient {Timeout = TimeSpan.FromSeconds(2)};
                var code = (int) (await httpClient.GetAsync(instance.InstanceAddress + instance.HealthCheckAddress)).StatusCode;
                return code >= 200 && code <= 299;
            }
            catch
            {
                return false;
            }
        }

        private static void RemoveFromFailed(IDictionary<string, int> failures, ServiceInstanceData instance)
        {
            if (failures.ContainsKey(instance.InstanceId))
            {
                failures.Remove(instance.InstanceId);
            }
        }

        private static void RemoveInstanceOnMaxFailure(IDictionary<string, int> failures, ServiceInstanceData instance,
            IInstanceRepository repository, ILogger logger)
        {
            if (failures[instance.InstanceId] == MaxFailures)
            {
                logger.LogInformation(
                    $"Reached max health check failures ({MaxFailures}). Removing instance {JsonConvert.SerializeObject(instance)}.");
                repository.Remove(instance.InstanceId);
            }
        }

        private static void IncreaseFailures(IDictionary<string, int> failures, ServiceInstanceData instance)
        {
            if (failures.ContainsKey(instance.InstanceId))
            {
                failures[instance.InstanceId]++;
            }
            else
            {
                failures[instance.InstanceId] = 1;
            }
        }
    }
}