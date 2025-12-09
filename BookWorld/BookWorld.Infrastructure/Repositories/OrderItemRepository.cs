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
    public class OrderItemRepository:IOrderItemRepository
    {
        private readonly BookWorldDbContext _context;

        public OrderItemRepository(BookWorldDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                                 .Include(oi => oi.Book)
                                 .Where(x => x.OrderId == orderId)
                                 .ToListAsync();
        }

        public async Task AddOrderItemAsync(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
           
        }

        public async Task<OrderItem> DeleteOrderItemAsync(OrderItem orderItem)
        {
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }
    }
}
