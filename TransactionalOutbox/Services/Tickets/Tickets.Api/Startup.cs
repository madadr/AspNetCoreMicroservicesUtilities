using System.Threading.Tasks;
using Common.Application.Commands;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tickets.Api.Extensions;
using Tickets.Application.Commands;
using Tickets.Application.Commands.Handlers;

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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.PublishesEvent<TicketBoughtIntegrationEvent>(logger).Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
