using BookWorld.Application.DTOs;
using BookWorld.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookWorld.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly OrderItemService _orderItemService;

        public OrderItemController(OrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var result = await _orderItemService.GetItemsByOrderIdAsync(orderId);
            return Ok(result);
        }

        [HttpPost("{orderId}")]
        public async Task<IActionResult> Add(int orderId, CreateOrderItemDto dto)
        {
            await _orderItemService.AddOrderItemAsync(orderId, dto);
            return Ok("Order item added");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderItemService.DeleteOrderItemAsync(id);
            return Ok("Order item deleted");
        }
    }
}
