using System;
using System.Threading.Tasks;
using Common.Application.Commands;
using Common.Application.EventBus;
using Common.Application.Events;
using Common.Infrastructure.Exceptions;
using EasyNetQ;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        public async Task PublishAsync<T>(T @event) where T : class, IEvent
        {
            _logger.LogInformation($"Publishing {@event.GetType()}:{JsonConvert.SerializeObject(@event)}");

            if (!_bus.IsConnected)
            {
                _logger.LogError($"Failed to publish {@event.GetType()} as broker not connected.");
                throw new InfrastructureException("Lost connection with event bus.");
            }

            try
            {
                await _bus.PublishAsync<T>(@event);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to publish {@event.GetType()}. Reason: {e.Message}.");
                throw new InfrastructureException($"Error during publish. Details: {e.Message}");
            }
        }

        public async Task PublishCommandAsync<T>(T command) where T : class, ICommand
        {
            _logger.LogInformation($"Publishing {command.GetType()}:{JsonConvert.SerializeObject(command)}");

            if (!_bus.IsConnected)
            {
                _logger.LogError($"Failed to publish {command.GetType()} as broker not connected.");
                throw new InfrastructureException("Lost connection with event bus.");
            }

            try
            {
                await _bus.PublishAsync<T>(command);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to publish {command.GetType()}. Reason: {e.Message}.");
                throw new InfrastructureException($"Error during publish. Details: {e.Message}");
            }
        }
    }
}