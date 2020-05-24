using Common.Api.Extensions;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Store.Application.Commands;
using Store.Application.Commands.Handlers;
using Store.Application.Events.Handlers;
using Store.Core.Repositories;
using Store.Infrastructure.Repositories;

namespace Store.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEventBus();
            services.AddSingleton(typeof(IReservationRepository), typeof(InMemoryReservationRepository));
            services.AddTransient(typeof(ICommandHandler<BookProduct>), typeof(BookProductCommandHandler));
            services.AddTransient(typeof(ICommandHandler<CancelProductBooking>),
                typeof(CancelProductBookingCommandHandler));
            services.AddTransient(typeof(IEventHandler<ClientVerified>), typeof(ClientVerifiedEventHandler));
            services.AddTransient(typeof(IEventHandler<PaymentFailed>), typeof(PaymentFailedEventHandler));
            services.AddTransient(typeof(IEventHandler<ProductReservationFailed>), typeof(ProductReservationFailedEventHandler));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.SubscribeEvent<ClientVerified>(logger, "Store");
            app.SubscribeEvent<PaymentFailed>(logger, "Store");
            app.SubscribeEvent<ProductReservationFailed>(logger, "Store");
        }
    }
}