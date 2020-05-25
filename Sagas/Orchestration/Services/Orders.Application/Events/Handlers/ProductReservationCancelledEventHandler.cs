using System;
using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orders.Application.Commands;

namespace Orders.Application.Events.Handlers
{
    public class ProductReservationCancelledEventHandler : EventHandlerBase<ProductReservationCancelled>
    {
        private readonly ICommandHandler<CancelOrder> _commandHandler;

        public ProductReservationCancelledEventHandler(ICommandHandler<CancelOrder> commandHandler,
            ILogger<ProductReservationCancelledEventHandler> logger) : base(logger)
        {
            _commandHandler = commandHandler;
        }

        public override async Task HandleAsync(ProductReservationCancelled @event)
        {
            await base.LogHandleAsync(@event);
            await _commandHandler.HandleAsync(new CancelOrder(@event.OrderId, @event.Reason, DateTime.Now));
        }
    }
}