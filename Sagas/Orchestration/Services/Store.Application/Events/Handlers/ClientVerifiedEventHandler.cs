using System.Threading.Tasks;
using Common.Application.Commands;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Store.Application.Commands;

namespace Store.Application.Events.Handlers
{
    public class ClientVerifiedEventHandler : EventHandlerBase<ClientVerified>
    {
        private readonly ICommandHandler<BookProduct> _commandHandler;

        public ClientVerifiedEventHandler(ICommandHandler<BookProduct> commandHandler,
            ILogger<ClientVerifiedEventHandler> logger) : base(logger)
        {
            _commandHandler = commandHandler;
        }

        public override async Task HandleAsync(ClientVerified @event)
        {
            await base.LogHandleAsync(@event);
            await _commandHandler.HandleAsync(new BookProduct(@event.OrderId, @event.ClientId, @event.ProductId,
                @event.Price));
        }
    }
}