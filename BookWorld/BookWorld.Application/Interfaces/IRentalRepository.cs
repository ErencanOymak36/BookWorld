using BookWorld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Application.Interfaces
{
    public interface IRentalRepository
    {
        Task CreateRentalAsync(Rental rental);
        Task UpdateRentalAsync(Rental rental);
        Task<Rental> DeleteRentalAsync(Rental rental);
        Task<Rental> GetRentalByIdAsync(int id);
        Task<IEnumerable<Rental>> GetAllRentalsAsync();
        Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(int userId);
    }
}
