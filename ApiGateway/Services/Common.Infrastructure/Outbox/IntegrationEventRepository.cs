using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Application.Events;
using Common.Application.Events.Outbox;
using MongoDB.Driver;

namespace Common.Infrastructure.Outbox
{
    public class IntegrationEventRepository<T> : IIntegrationEventRepository<T> where T : IIntegrationEvent
    {
        private readonly IMongoCollection<T> _collection;

        public IntegrationEventRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<T>(typeof(T).FullName);
        }

        public async Task AddAsync(T @event)
        {
            await _collection.InsertOneAsync(@event);
        }

        public async Task RemoveAsync(T @event)
        {
            await _collection.FindOneAndDeleteAsync(e => e.Id.Equals(@event.Id));
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return (await _collection.FindAsync(e => true)).ToEnumerable();
        }
    }
}