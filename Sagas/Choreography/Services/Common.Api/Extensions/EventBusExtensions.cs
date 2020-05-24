using System;
using System.Threading.Tasks;
using Common.Application.EventBus;
using Common.Application.Events;
using Common.Application.Events.Handlers;
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
            where T : class, IEvent
        {
            var subscriptionId = typeof(T).FullName + "." + serviceName;
            var eventHandler = (IEventHandler<T>) app.ApplicationServices.GetService(typeof(IEventHandler<T>));
            var bus = (IBus) app.ApplicationServices.GetService(typeof(IBus));

            Task.Run(async () => await SuperviseEventSubscriptionAsync(logger, bus, subscriptionId, eventHandler));
        }

        private static async Task SuperviseEventSubscriptionAsync<T>(ILogger logger, IBus bus, string subscriptionId,
            IEventHandler<T> eventHandler) where T : class, IEvent
        {
            // TODO: Investigate if EasyNetQ contains any subscriptions recovery after bus reconnection.
            bool wasConnectedDuringLastCheck = false;
            while (true)
            {
                if (bus.IsConnected && !wasConnectedDuringLastCheck)
                {
                    bus.Subscribe<T>(subscriptionId, async x => await eventHandler.HandleAsync(x));
                    logger.LogInformation($"Subscription created or recovered for event with subscription ID: {subscriptionId}");
                }

                wasConnectedDuringLastCheck = bus.IsConnected;
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }
    }
}