using System.Threading.Tasks;
using Common.Application.Commands;
using Common.Application.Commands.Handlers;
using Common.Application.EventBus;
using Common.Application.Events;

namespace Tickets.Application.Commands.Handlers
{
    public class BuyTicketCommandHandler : ICommandHandler<BuyTicketCommand>
    {
        private readonly IMessageBroker _messageBroker;

        public BuyTicketCommandHandler(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(BuyTicketCommand command)
        {
            await ProcessBuyTicketTransactionAsync();
            await _messageBroker.PublishAsync(new TicketBoughtEvent(command.CustomerId, command.MovieId, command.Seat));
        }

        private async Task ProcessBuyTicketTransactionAsync()
        {
            await Task.CompletedTask;
        }
    }
}