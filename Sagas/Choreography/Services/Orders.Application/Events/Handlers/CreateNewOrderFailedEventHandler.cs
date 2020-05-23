using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Common.Application.Events.Handlers;
using Orders.Application.Commands;

namespace Orders.Application.Events.Handlers
{
    public class CreateNewOrderFailedEventHandler : IEventHandler<CreateNewOrderFailed>
    {
        private readonly ICommandHandler<CancelOrder> _commandHandler;

        public CreateNewOrderFailedEventHandler(ICommandHandler<CancelOrder> commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public Task HandleAsync(CreateNewOrderFailed @event) =>
            _commandHandler.HandleAsync(new CancelOrder(@event.Id, @event.Reason));
    }
}