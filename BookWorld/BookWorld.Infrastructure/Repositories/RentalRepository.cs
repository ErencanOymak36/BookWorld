using BookWorld.Application.Interfaces;
using BookWorld.Domain.Entities;
using BookWorld.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Infrastructure.Repositories
{
    public class RentalRepository:IRentalRepository
    {
        private readonly BookWorldDbContext _context;

        public RentalRepository(BookWorldDbContext context)
        {
            _context = context;
        }

        public async Task CreateRentalAsync(Rental rental)
        {
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();
           
        }

        public async Task UpdateRentalAsync(Rental rental)
        {
            _context.Rentals.Update(rental);
            await _context.SaveChangesAsync();
            
        }

        public async Task<Rental> DeleteRentalAsync(Rental rental)
        {
            rental.IsDeleted = true;
            rental.DeletedAt = DateTime.UtcNow;

            _context.Rentals.Update(rental);
            await _context.SaveChangesAsync();

            return rental;
        }

        public async Task<Rental> GetRentalByIdAsync(int id)
        {
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Rental>> GetAllRentalsAsync()
        {
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Book)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(int userId)
        {
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> IsBookCurrentlyRentedAsync(int bookId)
        {
            return await _context.Rentals.AnyAsync(r => r.BookId == bookId && r.ReturnDate == null);
        }

        public async Task<IEnumerable<Rental>> GetActiveRentalsAsync()
        {
            return await _context.Rentals
                .Include(r => r.Book)
                .Include(r => r.User)
                .Where(r => r.ReturnDate == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetLateRentalsAsync()
        {
            return await _context.Rentals
                .Include(r => r.Book)
                .Include(r => r.User)
                .Where(r =>
                    r.ReturnDate == null &&
                    DateTime.UtcNow > r.RentDate.AddDays(r.RentalPeriodDays))
                .ToListAsync();
        }
    }
}
