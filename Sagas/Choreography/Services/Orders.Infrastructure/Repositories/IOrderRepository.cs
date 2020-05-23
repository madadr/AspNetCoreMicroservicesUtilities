using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.Core.Entities;
using Orders.Core.Repositories;

namespace Orders.Infrastructure.Repositories
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly IList<Order> _list = new List<Order>();

        public async Task AddAsync(Order order)
        {
            _list.Add(order);
            await Task.CompletedTask;
        }

        public async Task<Order> GetAsync(Guid id)
        {
            await Task.CompletedTask;
            return _list.FirstOrDefault(o => o.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            await Task.CompletedTask;
            return _list;
        }

        public async Task UpdateAsync(Order order)
        {
            if (await GetAsync(order.OrderId) is {})
            {
                await RemoveAsync(order.OrderId);
                await AddAsync(order);
            }
        }

        public async Task RemoveAsync(Guid id)
        {
            var order = await GetAsync(id);
            if (order != null)
            {
                _list.Remove(order);
            }
        }
    }
}