using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Router.Routing
{
    public class Router
    {
        private readonly ILogger _logger;
        private readonly IDictionary<string, ServiceState> _dictionary = new Dictionary<string, ServiceState>();

        private class ServiceState
        {
            public HashSet<string> InstanceAddresses { get; set; }
            public int NextInstanceIndex { get; set; }
        }

        public Router(ILogger<Router> logger)
        {
            _logger = logger;
        }

        public string GetNextPath(string serviceName)
        {
            var serviceState = _dictionary.ContainsKey(serviceName) ? _dictionary[serviceName] : null;
            if (serviceState == null)
            {
                _logger.LogInformation("No path to call");
                return null;
            }

            var address = serviceState.InstanceAddresses.ElementAt(serviceState.NextInstanceIndex);
            _logger.LogInformation($"Next path to call = {address}");
            serviceState.NextInstanceIndex =
                (serviceState.NextInstanceIndex + 1) % serviceState.InstanceAddresses.Count();
            return address;
        }

        public void UpdateStates(IDictionary<string, HashSet<string>> addresses)
        {
            _logger.LogInformation("Update states called");

            IList<string> current = new List<string>();
            if (addresses != null)
            {
                foreach (var service in addresses)
                {
                    current.Add(service.Key);
                    if (!_dictionary.ContainsKey(service.Key))
                    {
                        _logger.LogDebug("Adding empty ServiceState");
                        _dictionary[service.Key] = new ServiceState();
                    }

                    var serviceState = _dictionary[service.Key];
                    if (serviceState.InstanceAddresses != null && service.Value != null &&
                        service.Value.SetEquals(serviceState.InstanceAddresses))
                    {
                        _logger.LogDebug($"No change in service {service.Key}");
                        continue;
                    }

                    _logger.LogInformation(
                        $"Detected change in service {service.Key}. Previously: {JsonConvert.SerializeObject(serviceState.InstanceAddresses)}. Now: {JsonConvert.SerializeObject(service.Value)}");
                    serviceState.InstanceAddresses = service.Value;
                    serviceState.NextInstanceIndex = 0;
                }
            }

            foreach (var item in _dictionary)
            {
                if (!current.Contains(item.Key))
                {
                    _logger.LogDebug($"Removing service {item.Key}");
                    _dictionary.Remove(item.Key);
                }
            }
        }
    }
}