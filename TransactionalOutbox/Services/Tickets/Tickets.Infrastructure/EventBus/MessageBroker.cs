using System.Threading.Tasks;
using Tickets.Application.EventBus;
using Tickets.Application.Events;

namespace Tickets.Infrastructure.EventBus
{
    public class MessageBroker : IMessageBroker
    {
        public Task PublishAsync(IIntegrationEvent @event)
        {
            throw new System.NotImplementedException();
        }
    }
}