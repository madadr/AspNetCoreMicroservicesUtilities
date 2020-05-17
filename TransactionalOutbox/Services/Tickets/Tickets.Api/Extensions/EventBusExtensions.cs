using System.Threading.Tasks;
using Common.Application.EventBus;
using Common.Application.Events;
using Common.Infrastructure.EventBus;
using EasyNetQ;
using EasyNetQ.Management.Client;
using EasyNetQ.Management.Client.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tickets.Api.Extensions
{
    public static class StartupServicesExtensions
    {
        public static void AddEventBus(this IServiceCollection services)
        {
            // TODO: generate from app settings
            var connectionString =
                "host=localhost:5672;virtualHost=/;username=guest;password=guest";
            var bus = RabbitHutch.CreateBus(connectionString);
            services.AddSingleton(typeof(IBus), bus);
            var managementClient = new ManagementClient("http://localhost", "guest", "guest");
            services.AddSingleton(typeof(IManagementClient), managementClient);
            services.AddTransient(typeof(IMessageBroker), typeof(MessageBroker));
        }

        public static async Task PublishesEvent<T>(this IApplicationBuilder app, ILogger logger)
            where T : IIntegrationEvent
        {
            var topic = typeof(T).FullName + ".*";
            var managementClient = (IManagementClient) app.ApplicationServices.GetService(typeof(IManagementClient));
            await managementClient.CreateExchangeAsync(new ExchangeInfo(topic, "topic"),
                await managementClient.GetVhostAsync("/"));
            logger.LogInformation($"Service publishes events at topic {topic}");
        }
    }
}