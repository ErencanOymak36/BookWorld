using BookWorld.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace BookWorld.UI.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(
            IHttpClientFactory httpClientFactory,
            ILogger<AuthorController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // Yazar Ekleme Sayfası
        [HttpGet]
        public IActionResult CreateNewAuthor()
        {
            _logger.LogInformation("Yazar ekleme sayfası açıldı");
            return View();
        }

        // Yazar Kaydet
        [HttpPost]
        public async Task<IActionResult> CreateNewAuthor(CreateAuthorDto model)
        {
            _logger.LogInformation("Yazar ekleme işlemi başlatıldı");

            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("ApiClient");
            var content = new StringContent(JsonSerializer.Serialize(model),Encoding.UTF8,"application/json");
            var response = await client.PostAsync("api/author/CreateAuthor", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Yazar eklenemedi. StatusCode: {StatusCode}", response.StatusCode);
                ViewBag.Error = "Yazar eklenemedi";
                return View(model);
            }

            _logger.LogInformation("Yazar başarıyla eklendi");

            return RedirectToAction("Index", "Home");
        }
    }
}
