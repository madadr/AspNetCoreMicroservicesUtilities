using System.Threading.Tasks;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Microsoft.Extensions.Logging;

namespace MailNotifications.Application.Events.Handlers
{
    public class TicketBoughtEventHandler : IEventHandler<TicketBoughtEvent>
    {
        private ILogger<TicketBoughtEventHandler> _logger;

        public TicketBoughtEventHandler(ILogger<TicketBoughtEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(TicketBoughtEvent @event)
        {
            _logger.LogInformation($"TicketBoughtEventHandler: received event {@event}");
            await Task.CompletedTask;
        }
    }
}
