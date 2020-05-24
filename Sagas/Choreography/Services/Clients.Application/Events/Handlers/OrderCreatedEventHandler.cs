using System.Threading.Tasks;
using Clients.Application.Commands;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Microsoft.Extensions.Logging;

namespace Clients.Application.Events.Handlers
{
    public class OrderCreatedEventHandler : EventHandlerBase<OrderCreated>
    {
        private readonly ICommandHandler<VerifyClient> _commandHandler;

        public OrderCreatedEventHandler(ICommandHandler<VerifyClient> commandHandler,
            ILogger<OrderCreatedEventHandler> logger) : base(logger)
        {
            _commandHandler = commandHandler;
        }

        public override async Task HandleAsync(OrderCreated @event)
        {
            await base.LogHandleAsync(@event);
            await _commandHandler.HandleAsync(new VerifyClient(@event.Id, @event.ClientId, @event.ProductId,
                @event.Price));
        }
    }
}