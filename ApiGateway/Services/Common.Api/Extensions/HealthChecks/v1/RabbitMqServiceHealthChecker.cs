using EasyNetQ;

namespace Common.Api.Extensions.HealthChecks.v1
{
    public class RabbitMqServiceHealthChecker : IServiceHealthChecker
    {
        private readonly IBus _bus;
        public RabbitMqServiceHealthChecker(IBus bus) => _bus = bus;
        public string Name => "RabbitMQ"; 
        public bool IsHealthy => _bus.IsConnected;
        public string Details => _bus.IsConnected ? "Healthy" : "Not connected";
    }
}
