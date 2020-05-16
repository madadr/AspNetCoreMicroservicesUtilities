using System.Threading.Tasks;
using Tickets.Application.Events;

namespace Tickets.Application.EventBus
{
    public interface IMessageBroker
    {
        Task PublishAsync(IIntegrationEvent @event);
    }
}