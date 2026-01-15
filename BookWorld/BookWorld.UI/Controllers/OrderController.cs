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
 
            var client = _httpClientFactory.CreateClient("ApiClient");

            var orderDto = new CreateOrderDto
            {
              
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

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            _logger.LogInformation("Kullanıcının siparişleri getiriliyor");

            var client = _httpClientFactory.CreateClient("ApiClient");
           
            var response = await client.GetAsync($"api/order/getbyuser");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("Token geçersiz, logout yapılıyor");
                Response.Cookies.Delete("jwt");
                return RedirectToAction("Login", "Home");
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Siparişler alınamadı. StatusCode: {StatusCode}", response.StatusCode);
                ViewBag.Error = "Siparişler alınamadı";
                return View(new List<OrderDto>());
            }

            var json = await response.Content.ReadAsStringAsync();

            var orders = JsonSerializer.Deserialize<List<OrderDto>>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(orders);
        }


        [HttpGet]
        public async Task<IActionResult> OrderDetails(int orderId)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.GetAsync($"api/orderitem/{orderId}");

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

    }
}
