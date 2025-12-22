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
    public class OrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public OrderItemService(
            IOrderItemRepository orderItemRepository,
            IBookRepository bookRepository,
            IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderItemDto>> GetItemsByOrderIdAsync(int orderId)
        {
            var items = await _orderItemRepository.GetItemsByOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<OrderItemDto>>(items);
        }

        public async Task AddOrderItemAsync(int orderId, CreateOrderItemDto dto)
        {
            var book = await _bookRepository.GetBookByIdAsync(dto.BookId);

            var orderItem = new OrderItem
            {
                OrderId = orderId,
                BookId = dto.BookId,
                Quantity = dto.Quantity,
                UnitPrice = book.Price
            };

            await _orderItemRepository.AddOrderItemAsync(orderItem);
        }

        public async Task DeleteOrderItemAsync(int orderItemId)
        {
            var items = await _orderItemRepository.GetItemsByOrderIdAsync(0);
            var item = items.FirstOrDefault(x => x.Id == orderItemId);

            if (item != null)
                await _orderItemRepository.DeleteOrderItemAsync(item);
        }
    }
}
