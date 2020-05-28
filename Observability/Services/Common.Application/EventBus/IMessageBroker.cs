using System.Threading.Tasks;
using Common.Application.Events;

namespace Common.Application.EventBus
{
    public interface IMessageBroker
    {
        Task PublishAsync<T>(T @event) where T : class, IIntegrationEvent;
    }
}