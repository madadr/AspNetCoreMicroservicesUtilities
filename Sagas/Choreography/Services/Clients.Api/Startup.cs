using Clients.Application.Commands;
using Clients.Application.Commands.Handlers;
using Clients.Application.Events.Handlers;
using Common.Api.Extensions;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Clients.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEventBus();
            services.AddTransient(typeof(ICommandHandler<VerifyClient>), typeof(VerifyClientCommandHandler));
            services.AddTransient(typeof(IEventHandler<OrderCreated>), typeof(OrderCreatedEventHandler));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.SubscribeEvent<OrderCreated>(logger, "Clients");
        }
    }
}
