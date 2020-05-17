using System.Threading.Tasks;
using Common.Application.Commands;

namespace Common.Application.Events.Handlers
{
    public interface IEventHandler<T> where T : IIntegrationEvent
    {
        Task HandleAsync(T @event);
    }
}
