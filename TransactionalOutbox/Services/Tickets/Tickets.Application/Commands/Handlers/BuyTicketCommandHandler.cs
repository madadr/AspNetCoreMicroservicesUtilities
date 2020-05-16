using System;
using System.Threading.Tasks;
using Tickets.Application.EventBus;
using Tickets.Application.Events;

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
            await _messageBroker.PublishAsync(new TicketBoughtIntegrationEvent(command.CustomerId, command.Seat));
        }

        private async Task ProcessBuyTicketTransactionAsync()
        {
            // Simulate transaction processing
            await Task.Delay(TimeSpan.FromSeconds(0.5));
        }
    }
}