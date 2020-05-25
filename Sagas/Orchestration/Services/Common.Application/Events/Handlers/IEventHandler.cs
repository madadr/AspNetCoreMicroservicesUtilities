using System.Threading.Tasks;

namespace Common.Application.Events.Handlers
{
    public interface IEventHandler<in T> where T : IEvent
    {
        Task HandleAsync(T @event);
    }
}