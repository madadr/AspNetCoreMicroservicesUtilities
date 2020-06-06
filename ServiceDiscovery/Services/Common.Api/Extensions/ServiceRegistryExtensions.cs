using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Common.Api.Extensions
{
    public class ServiceInstanceData
    {
        public string ServiceName { get; set; }
        public string InstanceId { get; set; }
        public string InstanceAddress { get; set; }
        public bool HasHealthCheck { get; set; }
        public string HealthCheckAddress { get; set; }
    }

    public static class ServiceRegistryExtensions
    {
        private static string ServiceRegistryAddress = "http://service-registry:80/service";
        private const int Retries = 5;

        public static async Task UseServiceRegistry(this IApplicationBuilder app, IHostApplicationLifetime lifetime,
            ILogger logger, ServiceInstanceData data)
        {
            for (int attempt = 1; attempt <= Retries; ++attempt)
            {
                await Task.Delay(TimeSpan.FromSeconds((attempt - 1) * 5));
                try
                {
                    logger.LogInformation(
                        $"UseServiceRegistry (attempt {attempt}/{Retries}) called for {JsonConvert.SerializeObject(data)}");
                    if (!await RegisterServiceInstance(logger, data)) continue;
                }
                catch
                {
                    logger.LogWarning($"UseServiceRegistry failed due to exception.");
                    continue;
                }

                lifetime.ApplicationStopping.Register(async () =>
                {
                    var deregisterClient = new HttpClient();
                    await deregisterClient.DeleteAsync($"{ServiceRegistryAddress}/{data.InstanceId}");
                });
                return;
            }

            throw new Exception($"Service registration failed after {Retries} attempts");
        }

        private static async Task<bool> RegisterServiceInstance(ILogger logger, ServiceInstanceData data)
        {
            var registerClient = new HttpClient {Timeout = TimeSpan.FromSeconds(2)};
            var response = await registerClient.PostAsync(ServiceRegistryAddress,
                new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));

            if (response == null || (int) response.StatusCode < 200 || (int) response.StatusCode > 299)
            {
                logger.LogWarning($"UseServiceRegistry failed. Response {JsonConvert.SerializeObject(response)}");
                return false;
            }

            logger.LogInformation($"UseServiceRegistry succeeded. Response {JsonConvert.SerializeObject(response)}");
            return true;
        }
    }
}