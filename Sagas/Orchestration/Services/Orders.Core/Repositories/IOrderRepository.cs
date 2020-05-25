using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orders.Core.Entities;

namespace Orders.Core.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order> GetAsync(Guid id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task UpdateAsync(Order order);
        Task RemoveAsync(Guid id);
    }
}