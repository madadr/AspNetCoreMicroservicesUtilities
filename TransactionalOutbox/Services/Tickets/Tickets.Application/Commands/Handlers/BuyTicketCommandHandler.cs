using System;
using System.Threading.Tasks;
using Common.Application.Commands;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Outbox;
using Common.Application.TestApi;
using Microsoft.Extensions.Logging;
using Tickets.Application.TestApi;

namespace Tickets.Application.Commands.Handlers
{
    public class BuyTicketCommandHandler : ICommandHandler<BuyTicketCommand>
    {
        private readonly IIntegrationEventRepository<TicketBoughtEvent> _eventRepository;
        private readonly ILogger<BuyTicketCommandHandler> _logger;
        private readonly ICounter<IProcessedCommands> _processedCommandsCounter;

        public BuyTicketCommandHandler(IIntegrationEventRepository<TicketBoughtEvent> eventRepository,
            ILogger<BuyTicketCommandHandler> logger, ICounter<IProcessedCommands> processedCommandsCounter)
        {
            _eventRepository = eventRepository;
            _logger = logger;
            _processedCommandsCounter = processedCommandsCounter;
        }

        public async Task HandleAsync(BuyTicketCommand command)
        {
            UpdateTestApiCounter();

            _logger.LogDebug($"Processing command {command}");
            await ProcessBuyTicketTransactionAsync();
            var @event = new TicketBoughtEvent(Guid.NewGuid(), command.CustomerId, command.MovieId, command.Seat);
            _logger.LogDebug($"Enqueuing {@event}:{@event.Id}");
            await _eventRepository.AddAsync(@event);
        }

        private void UpdateTestApiCounter()
        {
            _processedCommandsCounter.Increment();
        }

        private async Task ProcessBuyTicketTransactionAsync()
        {
            await Task.CompletedTask;
        }
    }
}