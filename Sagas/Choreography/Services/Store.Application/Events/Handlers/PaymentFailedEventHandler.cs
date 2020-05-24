using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Store.Application.Commands;

namespace Store.Application.Events.Handlers
{
    public class PaymentFailedEventHandler : EventHandlerBase<PaymentFailed>
    {
        private readonly ICommandHandler<CancelProductBooking> _commandHandler;

        public PaymentFailedEventHandler(ICommandHandler<CancelProductBooking> commandHandler,
            ILogger<PaymentFailedEventHandler> logger) : base(logger)
        {
            _commandHandler = commandHandler;
        }

        public override async Task HandleAsync(PaymentFailed @event)
        {
            await base.LogHandleAsync(@event);
            await _commandHandler.HandleAsync(new CancelProductBooking(@event.OrderId, @event.ProductId, "PaymentFailed"));
        }
    }
}