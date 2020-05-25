using System;
using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Microsoft.Extensions.Logging;
using Orders.Application.Commands;

namespace Orders.Application.Events.Handlers
{
    public class CreateNewOrderFailedEventHandler : EventHandlerBase<CreateNewOrderFailed>
    {
        private readonly ICommandHandler<CancelOrder> _commandHandler;

        public CreateNewOrderFailedEventHandler(ICommandHandler<CancelOrder> commandHandler,
            ILogger<CreateNewOrderFailedEventHandler> logger) : base(logger)
        {
            _commandHandler = commandHandler;
        }

        public override async Task HandleAsync(CreateNewOrderFailed @event)
        {
            await base.LogHandleAsync(@event);
            await _commandHandler.HandleAsync(new CancelOrder(@event.Id, @event.Reason, DateTime.Now));
        }
    }
}