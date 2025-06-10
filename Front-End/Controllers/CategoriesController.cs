using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Front_End.Models;
using System.Net.Http;
using System.Text.Json;

namespace Front_End.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public CategoriesController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _clientFactory.CreateClient("BanHangDienMayAPI");
            try
            {
                var response = await client.GetAsync($"api/categories/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var category = JsonSerializer.Deserialize<CategoryDto>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (category == null || category.MaDanhMuc <= 0)
                    {
                        return NotFound("Danh mục không hợp lệ hoặc không tìm thấy.");
                    }
                    return View(category);
                }
                return NotFound($"Không tìm thấy danh mục với ID {id}. Mã trạng thái: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Lỗi kết nối đến backend: {ex.Message}");
            }
        }
    }
}