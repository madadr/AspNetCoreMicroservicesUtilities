using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Payments.Application.Commands;

namespace Payments.Application.Events.Handlers
{
    public class ProductReservedEventHandler : EventHandlerBase<ProductReserved>
    {
        private readonly ICommandHandler<ChargePayment> _commandHandler;

        public ProductReservedEventHandler(ICommandHandler<ChargePayment> commandHandler,
            ILogger<ProductReservedEventHandler> logger) : base(logger)
        {
            _commandHandler = commandHandler;
        }

        public override async Task HandleAsync(ProductReserved @event)
        {
            await base.LogHandleAsync(@event);
            await _commandHandler.HandleAsync(new ChargePayment(@event.OrderId, @event.ClientId, @event.ProductId,
                @event.Price));
        }
    }
}