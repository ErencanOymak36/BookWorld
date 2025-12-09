using BookWorld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task<Cart> GetCartByIdAsync(int id);
        Task CreateCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task<Cart> DeleteCartAsync(Cart cart);
    }
}
