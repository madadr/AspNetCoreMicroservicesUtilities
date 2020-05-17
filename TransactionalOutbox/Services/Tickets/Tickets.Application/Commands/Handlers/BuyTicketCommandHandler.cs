using System;
using System.Threading.Tasks;
using Common.Application.Commands;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Outbox;
using Microsoft.Extensions.Logging;

namespace Tickets.Application.Commands.Handlers
{
    public class BuyTicketCommandHandler : ICommandHandler<BuyTicketCommand>
    {
        private readonly IIntegrationEventRepository<TicketBoughtEvent> _eventRepository;
        private readonly ILogger<BuyTicketCommandHandler> _logger;

        public BuyTicketCommandHandler(IIntegrationEventRepository<TicketBoughtEvent> eventRepository,
            ILogger<BuyTicketCommandHandler> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public async Task HandleAsync(BuyTicketCommand command)
        {
            _logger.LogInformation($"Processing command {command}");
            await ProcessBuyTicketTransactionAsync();
            var @event = new TicketBoughtEvent(Guid.NewGuid(), command.CustomerId, command.MovieId, command.Seat);
            _logger.LogInformation($"Enqueuing {@event}:{@event.Id}");
            await _eventRepository.AddAsync(@event);
        }

        private async Task ProcessBuyTicketTransactionAsync()
        {
            await Task.CompletedTask;
        }
    }
}