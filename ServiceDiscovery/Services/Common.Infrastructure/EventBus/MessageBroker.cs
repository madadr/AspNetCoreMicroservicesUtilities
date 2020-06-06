using System;
using System.Threading.Tasks;
using Common.Application.EventBus;
using Common.Application.Events;
using Common.Infrastructure.Exceptions;
using EasyNetQ;
using Microsoft.Extensions.Logging;

namespace Common.Infrastructure.EventBus
{
    public class MessageBroker : IMessageBroker
    {
        private readonly ILogger<IMessageBroker> _logger;
        private readonly IBus _bus;

        public MessageBroker(ILogger<IMessageBroker> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task PublishAsync<T>(T @event) where T : class, IIntegrationEvent
        {
            _logger.LogDebug($"Requested publishing event of type: {@event.GetType()}");

            if (!_bus.IsConnected)
            {
                throw new InfrastructureException("Lost connection with event bus.");
            }

            try
            {
                await _bus.PublishAsync<T>(@event);
            }
            catch (Exception e)
            {
                throw new InfrastructureException($"Error during publish. Details: {e.Message}");
            }
        }
    }
}