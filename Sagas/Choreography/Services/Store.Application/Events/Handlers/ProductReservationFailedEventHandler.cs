using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Store.Application.Commands;

namespace Store.Application.Events.Handlers
{
    public class ProductReservationFailedEventHandler : EventHandlerBase<ProductReservationFailed>
    {
        private readonly ICommandHandler<CancelProductBooking> _commandHandler;

        public ProductReservationFailedEventHandler(ICommandHandler<CancelProductBooking> commandHandler,
            ILogger<ProductReservationFailedEventHandler> logger) : base(logger)
        {
            _commandHandler = commandHandler;
        }

        public override async Task HandleAsync(ProductReservationFailed @event)
        {
            await base.LogHandleAsync(@event);
            await _commandHandler.HandleAsync(new CancelProductBooking(@event.OrderId, @event.ProductId, "ProductReservationFailed"));
        }
    }
}