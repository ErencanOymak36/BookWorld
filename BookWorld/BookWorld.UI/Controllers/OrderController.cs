using BookWorld.Application.DTOs;
using BookWorld.UI.Services.Intefaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BookWorld.UI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OrderController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public OrderController(
            IHttpClientFactory httpClientFactory,
            ILogger<OrderController> logger,
            ICurrentUserService currentUserService)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public IActionResult CreateNewOrder(int bookId)
        {
            return View(new CreateOrderItemDto
            {
                BookId = bookId,
                Quantity = 1
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewOrder(CreateOrderItemDto model)
        {
            _logger.LogInformation("Sipariş oluşturma işlemi başlatıldı");

            //if (!_currentUserService.IsAuthenticated)
            //    return RedirectToAction("Login", "Auth");

            // var userId = _currentUserService.UserId;
            var userId = 1;

            var client = _httpClientFactory.CreateClient("ApiClient");

            var orderDto = new CreateOrderDto
            {
                UserId = userId, 
                OrderItems = new List<CreateOrderItemDto> { model }
            };

            var content = new StringContent(JsonSerializer.Serialize(orderDto),Encoding.UTF8,"application/json");

            var response = await client.PostAsync("api/order/CreateOrder",content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Sipariş oluşturulamadı. StatusCode: {StatusCode}",response.StatusCode);

                ViewBag.Error = "Sipariş oluşturulamadı";
                return View(model);
            }

            _logger.LogInformation("Sipariş başarıyla oluşturuldu");

            return RedirectToAction("Index", "Home");
        }
    }
}
