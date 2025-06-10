using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Front_End.Models;

namespace Front_End.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public CustomerController(IHttpClientFactory clientFactory)
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
                var response = await client.GetAsync($"api/customer/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var customer = JsonSerializer.Deserialize<KhachHang>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (customer == null || customer.MaKhachHang <= 0)
                    {
                        return NotFound("Khách hàng không hợp lệ hoặc không tìm thấy.");
                    }
                    return View(customer);
                }
                return NotFound($"Không tìm thấy khách hàng với ID {id}. Mã trạng thái: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Lỗi kết nối đến backend: {ex.Message}");
            }
        }
    }
}