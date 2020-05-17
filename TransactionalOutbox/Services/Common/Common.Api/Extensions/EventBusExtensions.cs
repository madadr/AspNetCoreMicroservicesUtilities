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
                "host=localhost:5672;virtualHost=/;username=guest;password=guest";
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

            // TODO: Investigate if EasyNetQ contains any subscriptions recovery after bus reconnection.
            Task.Run(async () =>
            {
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
            });
        }

        public static void PublishesEvent<T>(this IApplicationBuilder app, ILogger logger)
            where T : class, IIntegrationEvent
        {
            var eventRepository =
                (IIntegrationEventRepository<T>) app.ApplicationServices.GetService(
                    typeof(IIntegrationEventRepository<T>));

            var messageBroker = (IMessageBroker) app.ApplicationServices.GetService(typeof(IMessageBroker));
            var period = TimeSpan.FromSeconds(3);
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(period);
                    logger.LogInformation($"Periodic fetch of messages for type: {typeof(T).FullName}");
                    foreach (var @event in await eventRepository.GetAllAsync())
                    {
                        var eventId = $"{@event}:{@event.Id}";
                        try
                        {
                            logger.LogInformation($"Trying to publish: {eventId}");
                            await messageBroker.PublishAsync(@event);
                            logger.LogInformation($"Event published: {eventId}");
                            await eventRepository.RemoveAsync(@event);
                            logger.LogInformation($"Removed from store: {eventId}");
                        }
                        catch (Exception exception)
                        {
                            logger.LogInformation($"Failed to publish: {eventId}. Details: {exception.Message}");
                        }
                    }
                }
            });
            logger.LogInformation(
                $"Service will publish periodically ({period.ToString()}) events of type: {typeof(T).FullName}");
        }
    }
}