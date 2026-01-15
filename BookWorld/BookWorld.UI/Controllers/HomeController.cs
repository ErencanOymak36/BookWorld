using BookWorld.Application.DTOs;
using BookWorld.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BookWorld.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index sayfasý açýlýyor, kitaplar API'den çekilecek");
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync("api/book/GetAllBooks");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Books API çaðrýsý sýrasýnda hata oluþtu");
                ViewBag.Error = "Sunucuya baðlanýlamadý";
                return View(new List<BookDto>());
            }
          

            // Token geçersiz / süresi dolmuþsa
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("JWT geçersiz veya süresi dolmuþ, logout yapýlýyor");

                Response.Cookies.Delete("jwt");
                return RedirectToAction("Login");
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Books API baþarýsýz. StatusCode: {StatusCode}", response.StatusCode);
                ViewBag.Error = "Kitaplar alýnamadý";
                return View(new List<BookDto>());
            }

            // Data parse
            var json = await response.Content.ReadAsStringAsync();
            var books = JsonSerializer.Deserialize<List<BookDto>>(json,new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            _logger.LogInformation("Kitaplar baþarýyla çekildi. Count: {Count}", books?.Count ?? 0);

            return View(books);
        }

      

        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("Login sayfasý açýldý");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            _logger.LogInformation("Login denemesi baþladý. Email: {Email}", email);

            var client = _httpClientFactory.CreateClient("ApiClient");

            var content = new StringContent(JsonSerializer.Serialize(new { email, password }),Encoding.UTF8,"application/json");

            var response = await client.PostAsync("api/auth/login", content);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Auth API çaðrýsý baþarýsýz");
                ViewBag.Error = "Sunucuya ulaþýlamýyor";
                return View();
            }
           
            

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Error = "Email veya þifre hatalý";
                return View();
            }

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Bir hata oluþtu";
                return View();
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<Models.LoginResponseDto>(responseJson,new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Response.Cookies.Append("jwt", tokenResponse.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            _logger.LogInformation("Login baþarýlý. Email: {Email}", email);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _logger.LogInformation("Logout iþlemi baþlatýldý");

            if (Request.Cookies.ContainsKey("jwt"))
            {
                Response.Cookies.Delete("jwt", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                _logger.LogInformation("JWT cookie güvenli þekilde silindi");
            }
            else
            {
                _logger.LogWarning("JWT cookie bulunamadý, silinecek cookie yok");
            }

            return RedirectToAction("Login", "Home");
        }

    }
}
