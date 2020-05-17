using System.Threading.Tasks;
using Common.Application.EventBus;
using Common.Application.Events;
using Common.Infrastructure.Exceptions;
using EasyNetQ;
using EasyNetQ.Management.Client;
using Microsoft.Extensions.Logging;

namespace Common.Infrastructure.EventBus
{
    public class MessageBroker : IMessageBroker
    {
        private readonly ILogger<IMessageBroker> _logger;
        private readonly IBus _bus;
        private readonly IManagementClient _managementClient;

        public MessageBroker(ILogger<IMessageBroker> logger, IBus bus, IManagementClient managementClient)
        {
            _logger = logger;
            _bus = bus;
            _managementClient = managementClient;
        }

        public async Task PublishAsync<T>(T @event) where T : class, IIntegrationEvent
        {
            _logger.Log(LogLevel.Information, $"MessageBroker requested event of type: {@event.GetType()}");
            if (!_bus.IsConnected)
            {
                throw new InfrastructureException("Lost connection with event bus.");
            }

            await _bus.PublishAsync<T>(@event);
        }
    }
}