using Common.Api.Extensions;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Payments.Application.Commands;
using Payments.Application.Commands.Handlers;
using Payments.Application.Events.Handlers;

namespace Payments.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEventBus();
            services.AddTransient(typeof(ICommandHandler<ChargePayment>), typeof(ChargePaymentCommandHandler));
            services.AddTransient(typeof(IEventHandler<ProductReserved>), typeof(ProductReservedEventHandler));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.SubscribeEvent<ProductReserved>(logger, "Payments");
        }
    }
}