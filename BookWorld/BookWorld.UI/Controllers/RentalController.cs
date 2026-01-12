using BookWorld.Application.DTOs;
using BookWorld.UI.Models;
using BookWorld.UI.Services.Intefaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BookWorld.UI.Controllers
{
    public class RentalController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RentalController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public RentalController(
            IHttpClientFactory httpClientFactory,
            ILogger<RentalController> logger,
             ICurrentUserService currentUserService)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public IActionResult CreateNewRental(int bookId)
        {
            return View(new CreateRentalDto
            {
                BookId = bookId,
                RentalPeriodDays = 7
            });
        }


        [HttpPost]
        public async Task<IActionResult> CreateNewRental(CreateRentalDto model)
        {
            _logger.LogInformation("Kiralama işlemi başlatıldı");

            if (!_currentUserService.IsAuthenticated)
                return RedirectToAction("Login", "Auth");

            var userId = _currentUserService.UserId;

            var client = _httpClientFactory.CreateClient("ApiClient");

            var content = new StringContent(JsonSerializer.Serialize(model),Encoding.UTF8,"application/json");

            var response = await client.PostAsync("api/rental/CreateRental",content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Kiralama başarısız. StatusCode: {StatusCode}", response.StatusCode);
                ViewBag.Error = "Kiralama işlemi başarısız";
                return View(model);
            }

            _logger.LogInformation("Kiralama işlemi başarılı");

            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public IActionResult ReturnRentalBook(int rentalId, string? bookTitle)
        {
            _logger.LogInformation("İade onay sayfası açıldı. RentalId: {RentalId}", rentalId);

            var model = new ReturnRentalViewModel
            {
                RentalId = rentalId,
                BookTitle = bookTitle
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ReturnRental(int rentalId)
        {
            _logger.LogInformation("İade işlemi başlatıldı. RentalId: {RentalId}", rentalId);

            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PutAsync($"api/rental/returnbook/{rentalId}", null);

           

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "İade işlemi başarısız. StatusCode: {StatusCode}",
                    response.StatusCode);

                ViewBag.Error = "İade işlemi başarısız";
                return RedirectToAction("Index", "Home");
            }

            _logger.LogInformation("İade işlemi başarılı. RentalId: {RentalId}", rentalId);

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> MyRentals()
        {
            _logger.LogInformation("Kullanıcıya ait kiralamalar getiriliyor");

            if (!_currentUserService.IsAuthenticated)
                return RedirectToAction("Login", "Auth");

            var userId = _currentUserService.UserId;

            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.GetAsync($"api/rental/getbyuser/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Kiralama listesi alınamadı. StatusCode: {StatusCode}",
                    response.StatusCode);

                ViewBag.Error = "Kiralama bilgileri alınamadı";
                return View(new List<RentalDto>());
            }

            var json = await response.Content.ReadAsStringAsync();

            var rentals = JsonSerializer.Deserialize<List<RentalDto>>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            _logger.LogInformation("Kiralama listesi başarıyla alındı. Count: {Count}",rentals?.Count ?? 0);

            return View(rentals);
        }
    }
}
