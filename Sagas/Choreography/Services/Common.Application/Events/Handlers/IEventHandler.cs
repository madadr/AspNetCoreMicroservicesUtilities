using System.Threading.Tasks;

namespace Common.Application.Events.Handlers
{
    public interface IEventHandler<T> where T : IIntegrationEvent
    {
        Task HandleAsync(T @event);
    }
}