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
    public class CartItemRepository:ICartItemRepository
    {
        private readonly BookWorldDbContext _context;

        public CartItemRepository(BookWorldDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem> GetCartItemAsync(int cartId, int bookId)
        {
            return await _context.CartItems.Include(ci => ci.Book).FirstOrDefaultAsync(x => x.CartId == cartId && x.BookId == bookId);
        }

        public async Task<List<CartItem>> GetCartItemsByCartIdAsync(int cartId)
        {
            return await _context.CartItems
                                 .Include(ci => ci.Book)
                                 .Where(x => x.CartId == cartId)
                                 .ToListAsync();
        }

        public async Task AddToCartAsync(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
           
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            
        }

        public async Task<CartItem> DeleteCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }
    }
}
