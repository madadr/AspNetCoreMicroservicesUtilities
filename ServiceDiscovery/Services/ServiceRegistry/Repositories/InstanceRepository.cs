using System.Collections.Generic;
using System.Linq;
using ServiceRegistry.Controllers;

namespace ServiceRegistry.Repositories
{
    class InstanceRepository : IInstanceRepository
    {
        private readonly ISet<ServiceInstanceData> _collection = new HashSet<ServiceInstanceData>();

        public bool Contains(string instanceId)
        {
            return _collection.FirstOrDefault(data => data.InstanceId == instanceId) != null;
        }

        public void Add(ServiceInstanceData instanceData)
        {
            _collection.Add(instanceData);
        }

        public void Remove(string instanceId)
        {
            if (Contains(instanceId))
            {
                _collection.Remove(_collection.First(data => data.InstanceId == instanceId));
            }
        }

        public IEnumerable<string> GetServices()
        {
            return _collection.Select(item => item.ServiceName).ToHashSet();
        }

        public IEnumerable<ServiceInstanceData> GetInstanceStates()
        {
            return _collection;
        }

        public IEnumerable<string> GetServiceAddresses(string service)
        {
            return _collection.Where(i => i.ServiceName == service).Select(i => i.InstanceAddress);
        }
    }
}