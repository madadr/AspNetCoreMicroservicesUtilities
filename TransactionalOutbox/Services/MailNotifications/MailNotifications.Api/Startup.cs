using Common.Api.Extensions;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using MailNotifications.Application.Events.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MailNotifications.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IEventHandler<TicketBoughtEvent>), typeof(TicketBoughtEventHandler));
            services.AddEventBus();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.SubscribeEvent<TicketBoughtEvent>(logger, "MailNotifications"); // TODO: take serviceName from appSettings
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}