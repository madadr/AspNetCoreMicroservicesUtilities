using System.Collections.Generic;
using ServiceRegistry.Controllers;

namespace ServiceRegistry.Repositories
{
    public interface IInstanceRepository
    {
        bool Contains(string instanceDataInstanceId);
        void Add(ServiceInstanceData instanceData);
        void Remove(string instanceId);
        IEnumerable<string> GetServices();
        IEnumerable<ServiceInstanceData> GetInstanceStates();
        IEnumerable<string> GetServiceAddresses(string service);
    }
}