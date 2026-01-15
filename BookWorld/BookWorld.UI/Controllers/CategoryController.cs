using BookWorld.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace BookWorld.UI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(
            IHttpClientFactory httpClientFactory,
            ILogger<CategoryController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // Kategori Ekleme Sayfası
        [HttpGet]
        public IActionResult CreateNewCategory()
        {
            _logger.LogInformation("Kategori ekleme sayfası açıldı");
            return View();
        }

        // Kategori Kaydet
        [HttpPost]
        public async Task<IActionResult> CreateNewCategory(CreateCategoryDto model)
        {
            _logger.LogInformation("Kategori ekleme işlemi başlatıldı");

            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("ApiClient");

            var content = new StringContent(JsonSerializer.Serialize(model),Encoding.UTF8,"application/json");

            var response = await client.PostAsync("api/category/CreateCategory", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Kategori eklenemedi. StatusCode: {StatusCode}", response.StatusCode);
                ViewBag.Error = "Kategori eklenemedi";
                return View(model);
            }

            _logger.LogInformation("Kategori başarıyla eklendi");

            return RedirectToAction("Index", "Home");
        }
    }
}
