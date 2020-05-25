using Chronicle;
using Common.Api.Extensions;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orders.Api.Sagas;
using Orders.Application.Commands;
using Orders.Application.Commands.Handlers;
using Orders.Application.Events;
using Orders.Application.Events.Handlers;
using Orders.Core.Repositories;
using Orders.Infrastructure.Repositories;

namespace Orders.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEventBus();
            services.AddSingleton(typeof(IOrderRepository), typeof(InMemoryOrderRepository));
            services.AddTransient(typeof(ICommandHandler<PlaceOrder>), typeof(CreateOrderHandler));
            services.AddTransient(typeof(IEventHandler<ClientVerified>), typeof(CreateOrderHandler));
            services.AddTransient(typeof(IEventHandler<ClientNotPermitted>), typeof(CreateOrderHandler));
            services.AddTransient(typeof(IEventHandler<ProductReserved>), typeof(CreateOrderHandler));
            services.AddTransient(typeof(IEventHandler<ProductReservationFailed>), typeof(CreateOrderHandler));
            services.AddTransient(typeof(IEventHandler<PaymentAccepted>), typeof(CreateOrderHandler));
            services.AddTransient(typeof(IEventHandler<PaymentFailed>), typeof(CreateOrderHandler));
            services.AddChronicle();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.SubscribeEvent<ClientVerified>(logger, "Orders");
            app.SubscribeEvent<ClientNotPermitted>(logger, "Orders");
            app.SubscribeEvent<ProductReserved>(logger, "Orders");
            app.SubscribeEvent<ProductReservationFailed>(logger, "Orders");
            app.SubscribeEvent<PaymentAccepted>(logger, "Orders");
            app.SubscribeEvent<PaymentFailed>(logger, "Orders");

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