using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Router.Routing
{
    public static class Extensions
    {
        private static readonly string ServiceRegistryAddress = "http://service-registry:80/service";

        public static void SuperviseServiceRegistry(this IApplicationBuilder app, ILogger<Startup> logger)
        {
            logger.LogInformation("Starting SuperviseServiceRegistry");
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(10));
                    logger.LogInformation("Periodic service registry supervision");
                    var client = new HttpClient{Timeout = TimeSpan.FromSeconds(2)};
                    var response = await client.GetAsync(ServiceRegistryAddress);
                    if (response == null)
                    {
                        logger.LogDebug($"Null response");
                        app.ApplicationServices.GetService<Router>().UpdateStates(null);
                        return;
                    }
                    logger.LogTrace($"Received not null response: {await response.Content.ReadAsStringAsync()}");
                    var instanceDatas =
                        JsonConvert.DeserializeObject<List<ServiceInstanceData>>(
                            await response.Content.ReadAsStringAsync());
                    if (instanceDatas == null)
                    {
                        logger.LogDebug($"Null instanceDatas");
                        app.ApplicationServices.GetService<Router>().UpdateStates(null);
                        return;
                    }

                    IDictionary<string, HashSet<string>> addresses = new Dictionary<string, HashSet<string>>();
                    foreach (var instanceData in instanceDatas)
                    {
                        if (!addresses.ContainsKey(instanceData.ServiceName))
                        {
                            addresses.Add(instanceData.ServiceName, new HashSet<string>());
                        }

                        addresses[instanceData.ServiceName].Add(instanceData.InstanceAddress);
                    }

                    app.ApplicationServices.GetService<Router>()?.UpdateStates(addresses);
                }
            });
        }

        private class ServiceInstanceData
        {
            public string ServiceName { get; set; }
            public string InstanceId { get; set; }
            public string InstanceAddress { get; set; }
            public bool HasHealthCheck { get; set; }
            public string HealthCheckAddress { get; set; }
        }
    }
}