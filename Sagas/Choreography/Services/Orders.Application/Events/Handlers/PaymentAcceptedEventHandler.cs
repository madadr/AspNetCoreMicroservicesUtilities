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
    public class PaymentAcceptedEventHandler : EventHandlerBase<PaymentAccepted>
    {
        private readonly ICommandHandler<ApproveOrder> _commandHandler;

        public PaymentAcceptedEventHandler(ICommandHandler<ApproveOrder> commandHandler,
            ILogger<PaymentAcceptedEventHandler> logger) : base(logger)
        {
            _commandHandler = commandHandler;
        }

        public override async Task HandleAsync(PaymentAccepted @event)
        {
            await base.LogHandleAsync(@event);
            await _commandHandler.HandleAsync(new ApproveOrder(@event.OrderId, DateTime.Now));
        }
    }
}