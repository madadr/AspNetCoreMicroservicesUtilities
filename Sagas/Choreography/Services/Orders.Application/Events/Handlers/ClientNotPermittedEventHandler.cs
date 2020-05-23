using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Orders.Application.Commands;

namespace Orders.Application.Events.Handlers
{
    public class ClientNotPermittedEventHandler : IEventHandler<ClientNotPermitted>
    {
        private readonly ICommandHandler<CancelOrder> _commandHandler;

        public ClientNotPermittedEventHandler(ICommandHandler<CancelOrder> commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public Task HandleAsync(ClientNotPermitted @event) =>
            _commandHandler.HandleAsync(new CancelOrder(@event.OrderId, @event.Reason));
    }
}