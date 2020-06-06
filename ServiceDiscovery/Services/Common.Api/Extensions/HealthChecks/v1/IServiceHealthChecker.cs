using Common.Application.EventBus;

namespace Common.Api.Extensions.HealthChecks.v1
{
    public interface IServiceHealthChecker
    {
        string Name { get; }
        bool IsHealthy { get; }
        string Details { get; }
    }
    
}