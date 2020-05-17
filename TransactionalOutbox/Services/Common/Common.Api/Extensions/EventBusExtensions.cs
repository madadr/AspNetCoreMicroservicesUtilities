using Common.Application.EventBus;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Common.Infrastructure.EventBus;
using EasyNetQ;
using EasyNetQ.Management.Client;
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
            var managementClient = new ManagementClient("http://localhost", "guest", "guest");
            services.AddSingleton(typeof(IManagementClient), managementClient);
            services.AddTransient(typeof(IMessageBroker), typeof(MessageBroker));
        }

        public static void SubscribeEvent<T>(this IApplicationBuilder app, ILogger logger, string serviceName)
            where T : class, IIntegrationEvent
        {
            var subscriptionId = typeof(T).FullName + "." + serviceName;
            var eventHandler = (IEventHandler<T>) app.ApplicationServices.GetService(typeof(IEventHandler<T>));
            var bus = (IBus) app.ApplicationServices.GetService(typeof(IBus));
            bus.Subscribe<T>(subscriptionId, async x => await eventHandler.HandleAsync(x));
            logger.LogInformation($"Service subscribed for event with subscription ID: {subscriptionId}");
        }
    }
}