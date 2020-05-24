using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Common.Application.Events.Handlers
{
    public abstract class EventHandlerBase<T> : IEventHandler<T> where T : IEvent
    {
        protected readonly ILogger<EventHandlerBase<T>> Logger;

        protected EventHandlerBase(ILogger<EventHandlerBase<T>> logger)
        {
            Logger = logger;
        }

        public async Task LogHandleAsync(T @event)
        {
            await Task.Run(() => Logger.LogInformation($"Handling {@event}: {JsonConvert.SerializeObject(@event)}"));
        }

        public abstract Task HandleAsync(T @event);
    }
}