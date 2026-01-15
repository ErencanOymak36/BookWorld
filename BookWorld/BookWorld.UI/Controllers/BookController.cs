using BookWorld.Application.DTOs;
using BookWorld.UI.Services.Intefaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace BookWorld.UI.Controllers
{
    public class BookController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BookController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public BookController(
            IHttpClientFactory httpClientFactory,
            ILogger<BookController> logger,
            ICurrentUserService currentUserService)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;

        }

        [HttpGet]
        public async Task<IActionResult> CreateNewBook()
        {
            _logger.LogInformation("Kitap ekleme sayfası açıldı");

            var client = _httpClientFactory.CreateClient("ApiClient");

            // ===================== AUTHORS =====================
            var authorResponse = await client.GetAsync("api/author/getall");

            if (!authorResponse.IsSuccessStatusCode)
            {
                _logger.LogError("Yazarlar alınamadı. StatusCode: {StatusCode}", authorResponse.StatusCode);
                ViewBag.Error = "Yazarlar alınamadı";
                return View();
            }

            var authorJson = await authorResponse.Content.ReadAsStringAsync();

            var authors = JsonSerializer.Deserialize<List<AuthorDto>>(authorJson,new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            ViewBag.Authors = authors ?? new List<AuthorDto>();

            // ===================== CATEGORIES =====================
            var categoryResponse = await client.GetAsync("api/category/getallcategories");

            if (!categoryResponse.IsSuccessStatusCode)
            {
                _logger.LogError("Kategoriler alınamadı. StatusCode: {StatusCode}", categoryResponse.StatusCode);
                ViewBag.Error = "Kategoriler alınamadı";
                return View();
            }

            var categoryJson = await categoryResponse.Content.ReadAsStringAsync();

            var categories = JsonSerializer.Deserialize<List<CategoryDto>>(categoryJson,new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            ViewBag.Categories = categories ?? new List<CategoryDto>();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateNewBook(CreateBookDto model)
        {
            _logger.LogInformation("Kitap ekleme işlemi başlatıldı");

            var client = _httpClientFactory.CreateClient("ApiClient");

            var content = new StringContent(JsonSerializer.Serialize(model),Encoding.UTF8,"application/json");

            var response = await client.PostAsync("api/book/CreateBook", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Kitap ekleme başarısız. StatusCode: {StatusCode}", response.StatusCode);
                ViewBag.Error = "Kitap eklenemedi";

                // Yazarları tekrar doldur
                var authorsResponse = await client.GetAsync("api/author/getall");
                var authorsJson = await authorsResponse.Content.ReadAsStringAsync();
                ViewBag.Authors = JsonSerializer.Deserialize<List<AuthorDto>>(authorsJson);

                return View(model);
            }

            _logger.LogInformation("Kitap başarıyla eklendi");

            return RedirectToAction("Index", "Home");
        }
    }
}
