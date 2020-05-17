using System.Threading.Tasks;
using Common.Application.Events;

namespace Common.Application.EventBus
{
    public interface IMessageBroker
    {
        Task PublishAsync(IIntegrationEvent @event);
    }
}