using AutoMapper;
using BookWorld.Application.DTOs;
using BookWorld.Application.Interfaces;
using BookWorld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IBookRepository bookRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task CreateOrderAsync(CreateOrderDto dto)
        {
            var order = new Order
            {
                UserId = dto.UserId,
                OrderDate = DateTime.UtcNow,
                Status = "Created",
                TotalAmount = 0,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in dto.OrderItems)
            {
                var book = await _bookRepository.GetBookByIdAsync(item.BookId);

                if (book.Stock < item.Quantity)
                    throw new Exception($"{book.Title} için yeterli stok yok");

                // Stok düş
                book.Stock -= item.Quantity;

                var orderItem = new OrderItem
                {
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    UnitPrice = book.Price
                };

                order.TotalAmount += book.Price * item.Quantity;
                order.OrderItems.Add(orderItem);
                
                _bookRepository.UpdateBookAsync(book);
            }

            await _orderRepository.CreateOrderAsync(order);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order != null)
                await _orderRepository.DeleteOrderAsync(order);
        }
    }
}
