namespace ServiceRegistry.Controllers
{
    public class ServiceInstanceData
    {
        public string ServiceName { get; set; }
        public string InstanceId { get; set; }
        public string InstanceAddress { get; set; }
        public bool HasHealthCheck { get; set; }
        public string HealthCheckAddress { get; set; }
    }
}