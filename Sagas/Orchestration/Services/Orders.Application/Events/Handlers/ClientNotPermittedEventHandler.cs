using System;
using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Microsoft.Extensions.Logging;
using Orders.Application.Commands;

namespace Orders.Application.Events.Handlers
{
    public class ClientNotPermittedEventHandler : EventHandlerBase<ClientNotPermitted>
    {
        private readonly ICommandHandler<CancelOrder> _commandHandler;

        public ClientNotPermittedEventHandler(ICommandHandler<CancelOrder> commandHandler,
            ILogger<ClientNotPermittedEventHandler> logger) : base(logger)
        {
            _commandHandler = commandHandler;
        }

        public override async Task HandleAsync(ClientNotPermitted @event)
        {
            await base.LogHandleAsync(@event);
            await _commandHandler.HandleAsync(new CancelOrder(@event.OrderId, @event.Reason, DateTime.Now));
        }
    }
}