using Common.Api.Extensions;
using Common.Api.Extensions.HealthChecks.v1;
using Common.Application.Commands;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Outbox;
using Common.Infrastructure.Outbox;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tickets.Application.Commands.Handlers;
using Tickets.Application.TestApi;

namespace Tickets.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient(typeof(ICommandHandler<BuyTicketCommand>), typeof(BuyTicketCommandHandler));
            services.AddEventBus();
            services.AddMongo();
            services.AddSingleton(typeof(IIntegrationEventRepository<TicketBoughtEvent>),
                typeof(IntegrationEventRepository<TicketBoughtEvent>));

            // Test API
            services.AddCounter<IProcessedCommands>();

            services.AddTransient(typeof(IServiceHealthChecker), typeof(RabbitMqServiceHealthChecker));
            services.AddTransient(typeof(IServiceHealthChecker), typeof(MongoDbServiceHealthChecker));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Configure called");
            app.PublishesEvent<TicketBoughtEvent>(logger);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                logger.LogInformation("Mapping HC");
                endpoints.MapCommonHealthChecks(app);
                logger.LogInformation("Mapping Controllers");
                endpoints.MapControllers();
            });
        }
    }
}