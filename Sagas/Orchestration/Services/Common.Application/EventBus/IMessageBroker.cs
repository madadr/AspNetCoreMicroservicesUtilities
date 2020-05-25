using System.Threading.Tasks;
using Common.Application.Commands;
using Common.Application.Events;

namespace Common.Application.EventBus
{
    public interface IMessageBroker
    {
        Task PublishAsync<T>(T @event) where T : class, IEvent;
        Task PublishCommandAsync<T>(T command) where T : class, ICommand;
    }
}