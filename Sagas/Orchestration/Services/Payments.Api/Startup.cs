using Common.Api.Extensions;
using Common.Application.Commands;
using Common.Application.Commands.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Payments.Application.Commands.Handlers;

namespace Payments.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEventBus();
            services.AddTransient(typeof(ICommandHandler<ChargePayment>), typeof(ChargePaymentCommandHandler));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.SubscribeCommand<ChargePayment>(logger, "Payments");
        }
    }
}