using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Front_End.Models;
using System.Net.Http;
using System.Text.Json;

namespace Front_End.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public ProductsController(IHttpClientFactory clientFactory)
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
                var response = await client.GetAsync($"api/products/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var product = JsonSerializer.Deserialize<SanPham>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (product == null || product.MaSanPham <= 0)
                    {
                        return NotFound("Sản phẩm không hợp lệ hoặc không tìm thấy.");
                    }
                    return View(product);
                }
                return NotFound($"Không tìm thấy sản phẩm với ID {id}. Mã trạng thái: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Lỗi kết nối đến backend: {ex.Message}");
            }
        }
    }
}