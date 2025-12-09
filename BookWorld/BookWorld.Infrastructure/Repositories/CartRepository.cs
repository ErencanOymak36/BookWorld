using BookWorld.Application.Interfaces;
using BookWorld.Domain.Entities;
using BookWorld.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace BookWorld.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly BookWorldDbContext _context;

        public CartRepository(BookWorldDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Book).FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<Cart> GetCartByIdAsync(int id)
        {
            return await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Book).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateCartAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            
        }

        public async Task<Cart> DeleteCartAsync(Cart cart)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
    }
}
