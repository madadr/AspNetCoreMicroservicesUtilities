using Common.Api.Extensions;
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
using Microsoft.OpenApi.Models;
using Tickets.Api.TestApi;
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
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
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
                endpoints.MapControllers();
            });
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
} 