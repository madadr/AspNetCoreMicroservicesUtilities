using System.Threading.Tasks;
using Common.Application.EventBus;
using Common.Application.Events;
using Common.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;
using EasyNetQ;
using EasyNetQ.Management.Client;
using EasyNetQ.Management.Client.Model;

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

        public async Task PublishAsync(IIntegrationEvent @event)
        {
            _logger.Log(LogLevel.Information, $"MessageBroker requested event of type: {@event.GetType()}");
            if (!_bus.IsConnected)
            {
                throw new InfrastructureException("Lost connection with event bus.");
            }

            var topic = @event.GetType() + ".*";
            var exchange = await _managementClient.CreateExchangeAsync(new ExchangeInfo(topic, "topic"),
                await _managementClient.GetVhostAsync("/"));
            await _bus.PublishAsync(@event, topic);
        }
    }
}