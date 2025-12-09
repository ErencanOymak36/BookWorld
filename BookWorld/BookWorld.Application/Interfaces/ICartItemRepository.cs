using BookWorld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Application.Interfaces
{
    public interface ICartItemRepository
    {
        Task<CartItem> GetCartItemAsync(int cartId, int bookId);
        Task<List<CartItem>> GetCartItemsByCartIdAsync(int cartId);
        Task AddToCartAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task<CartItem> DeleteCartItemAsync(CartItem cartItem);
    }
}
