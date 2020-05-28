using System;
using System.Threading.Tasks;
using Common.Application.EventBus;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Common.Application.Events.Outbox;
using Common.Infrastructure.EventBus;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Api.Extensions
{
    public static class StartupServicesExtensions
    {
        public static void AddEventBus(this IServiceCollection services)
        {
            // TODO: generate connectionString from appSettings
            var connectionString =
                "host=rabbitmq:5672;virtualHost=/;username=guest;password=guest";
            var bus = RabbitHutch.CreateBus(connectionString);
            services.AddSingleton(typeof(IBus), bus);
            services.AddTransient(typeof(IMessageBroker), typeof(MessageBroker));
        }

        public static void SubscribeEvent<T>(this IApplicationBuilder app, ILogger logger, string serviceName)
            where T : class, IIntegrationEvent
        {
            var subscriptionId = typeof(T).FullName + "." + serviceName;
            var eventHandler = (IEventHandler<T>) app.ApplicationServices.GetService(typeof(IEventHandler<T>));
            var bus = (IBus) app.ApplicationServices.GetService(typeof(IBus));

            Task.Run(async () => await SuperviseEventSubscriptionAsync(logger, bus, subscriptionId, eventHandler));
        }

        private static async Task SuperviseEventSubscriptionAsync<T>(ILogger logger, IBus bus, string subscriptionId,
            IEventHandler<T> eventHandler) where T : class, IIntegrationEvent
        {
            // TODO: Investigate if EasyNetQ contains any subscriptions recovery after bus reconnection.
            bool wasConnectedDuringLastCheck = false;
            while (true)
            {
                if (bus.IsConnected && !wasConnectedDuringLastCheck)
                {
                    bus.Subscribe<T>(subscriptionId, async x => await eventHandler.HandleAsync(x));
                    logger.LogInformation(
                        $"Subscription created or recovered for event with subscription ID: {subscriptionId}");
                }

                wasConnectedDuringLastCheck = bus.IsConnected;
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }

        public static void PublishesEvent<T>(this IApplicationBuilder app, ILogger logger)
            where T : class, IIntegrationEvent
        {
            var eventRepository =
                (IIntegrationEventRepository<T>) app.ApplicationServices.GetService(
                    typeof(IIntegrationEventRepository<T>));

            var messageBroker = (IMessageBroker) app.ApplicationServices.GetService(typeof(IMessageBroker));
            var period = TimeSpan.FromSeconds(3);
            Task.Run(async () => await PublishUnprocessedEventsPeriodicallyAsync(logger, period, eventRepository, messageBroker));
            logger.LogDebug(
                $"Service will publish periodically ({period.ToString()}) events of type: {typeof(T).FullName}");
        }

        private static async Task PublishUnprocessedEventsPeriodicallyAsync<T>(ILogger logger, TimeSpan period,
            IIntegrationEventRepository<T> eventRepository, IMessageBroker messageBroker) where T : class, IIntegrationEvent
        {
            while (true)
            {
                await Task.Delay(period);
                await PublishUnprocessedEventsAsync(logger, eventRepository, messageBroker);
            }
        }

        private static async Task PublishUnprocessedEventsAsync<T>(ILogger logger,
            IIntegrationEventRepository<T> eventRepository, IMessageBroker messageBroker) where T : class, IIntegrationEvent
        {
            logger.LogDebug($"Periodic fetch of messages for type: {typeof(T).FullName}");
            foreach (var @event in await eventRepository.GetAllAsync())
            {
                var eventId = $"{@event}:{@event.Id}";
                try
                {
                    logger.LogDebug($"Trying to publish: {eventId}");
                    await messageBroker.PublishAsync(@event);
                    logger.LogDebug($"Event published: {eventId}");
                    await eventRepository.RemoveAsync(@event);
                    logger.LogDebug($"Removed from store: {eventId}");
                }
                catch (Exception exception)
                {
                    logger.LogError($"Failed to publish: {eventId}. Details: {exception.Message}");
                }
            }
        }
    }
}