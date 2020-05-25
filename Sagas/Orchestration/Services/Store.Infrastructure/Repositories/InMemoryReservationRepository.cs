using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store.Core.Entities;
using Store.Core.Repositories;

namespace Store.Infrastructure.Repositories
{
    public class InMemoryReservationRepository : IReservationRepository
    {
        private readonly IList<Reservation> _list = new List<Reservation>();

        public async Task AddAsync(Reservation reservation)
        {
            _list.Add(reservation);
            await Task.CompletedTask;
        }

        public async Task<Reservation> GetAsync(int productId)
        {
            await Task.CompletedTask;
            return _list.FirstOrDefault(o => o.ProductId == productId);
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            await Task.CompletedTask;
            return _list;
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            if (await GetAsync(reservation.ProductId) is {})
            {
                await RemoveAsync(reservation.ProductId);
                await AddAsync(reservation);
            }
        }

        public async Task RemoveAsync(int productId)
        {
            var order = await GetAsync(productId);
            if (order != null)
            {
                _list.Remove(order);
            }
        }
    }
}