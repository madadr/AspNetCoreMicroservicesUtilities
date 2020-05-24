using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Store.Core.Entities;

namespace Store.Core.Repositories
{
    public interface IReservationRepository
    {
        Task AddAsync(Reservation reservation);
        Task<Reservation> GetAsync(int productId);
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task UpdateAsync(Reservation reservation);
        Task RemoveAsync(int productId);
    }
}