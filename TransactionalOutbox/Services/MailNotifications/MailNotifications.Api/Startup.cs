using Common.Api.Extensions;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using MailNotifications.Api.TestApi;
using MailNotifications.Application.Events.Handlers;
using MailNotifications.Application.TestApi;
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
            services.AddControllers();
            services.AddTransient(typeof(IEventHandler<TicketBoughtEvent>), typeof(TicketBoughtEventHandler));
            services.AddEventBus();

            // Test API
            services.AddCounter<IReceivedEvents>();
            services.AddCounter<IUniqueReceivedEvents>();
            services.AddSingleton(typeof(EventIdsRepository));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.SubscribeEvent<TicketBoughtEvent>(logger, "MailNotifications"); // TODO: take serviceName from appSettings
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}