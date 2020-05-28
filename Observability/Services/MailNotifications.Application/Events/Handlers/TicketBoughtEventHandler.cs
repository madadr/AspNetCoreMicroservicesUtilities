using System;
using System.Threading.Tasks;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Common.Application.TestApi;
using MailNotifications.Application.TestApi;
using Microsoft.Extensions.Logging;

namespace MailNotifications.Application.Events.Handlers
{
    public class TicketBoughtEventHandler : IEventHandler<TicketBoughtEvent>
    {
        private readonly ILogger<TicketBoughtEventHandler> _logger;
        private readonly ICounter<IReceivedEvents> _receivedEventsCounter;
        private readonly ICounter<IUniqueReceivedEvents> _uniqueReceivedEventsCounter;
        private readonly EventIdsRepository _eventIdsRepository;

        public TicketBoughtEventHandler(ILogger<TicketBoughtEventHandler> logger,
            ICounter<IReceivedEvents> receivedEventsCounter,
            ICounter<IUniqueReceivedEvents> uniqueReceivedEventsCounter,
            EventIdsRepository eventIdsRepository)
        {
            _logger = logger;
            
            _receivedEventsCounter = receivedEventsCounter;
            _uniqueReceivedEventsCounter = uniqueReceivedEventsCounter;
            _eventIdsRepository = eventIdsRepository;
        }

        public async Task HandleAsync(TicketBoughtEvent @event)
        {
            UpdateTestApiCounters(@event.Id);

            _logger.LogDebug($"TicketBoughtEventHandler: received event {@event}:{@event.Id}");
            await Task.CompletedTask;
        }

        private void UpdateTestApiCounters(Guid id)
        {
            _receivedEventsCounter.Increment();
            if (_eventIdsRepository.Contains(id))
            {
                return;
            }
            _eventIdsRepository.Add(id);
            _uniqueReceivedEventsCounter.Increment();
        }
    }
}