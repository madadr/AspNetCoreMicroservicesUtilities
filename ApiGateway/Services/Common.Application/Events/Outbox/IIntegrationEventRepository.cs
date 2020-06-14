using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Application.Events.Outbox
{
    public interface IIntegrationEventRepository<T> where T : IIntegrationEvent
    {
        Task AddAsync(T @event);
        Task RemoveAsync(T @event);
        Task<IEnumerable<T>> GetAllAsync();
    }
}