using Microsoft.AspNetCore.Mvc;
using Front_End.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace Front_End.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WarehouseController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }



        // GET: Hiển thị form nhập kho
        public async Task<IActionResult> InsertWarehouse()
        {
            var client = _httpClientFactory.CreateClient("BanHangDienMayAPI");

            var response = await client.GetAsync("api/Warehouse/sanpham");

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var sanPhams = await response.Content.ReadFromJsonAsync<List<SanPham>>();
                    if (sanPhams == null)
                    {
                        ViewBag.Error = "Dữ liệu trả về là null sau khi parse JSON.";
                        return View(new List<SanPham>());
                    }
                    return View(sanPhams);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Lỗi khi parse JSON: " + ex.Message;
                    return View(new List<SanPham>());
                }
            }
            else
            {
                ViewBag.Error = "Không thể gọi API, mã lỗi: " + response.StatusCode;
                return View(new List<SanPham>());
            }
        }

        // POST: Gửi dữ liệu số lượng nhập và gọi API backend
        [HttpPost]
        public async Task<IActionResult> CapNhat([FromForm] Dictionary<int, int> soLuongNhap)
        {
            var client = _httpClientFactory.CreateClient("BanHangDienMayAPI");

            var response = await client.PostAsJsonAsync("api/Warehouse/capnhat", soLuongNhap);
            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { message = "API lỗi: " + msg });
            }

            return Ok(new { message = "Cập nhật nhập kho thành công!" });
        }
        // GET: Trang kiểm kho
        public async Task<IActionResult> CheckWarehouse()
        {
            var client = _httpClientFactory.CreateClient("BanHangDienMayAPI");

            var response = await client.GetAsync("api/Warehouse/sanpham");

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var sanPhams = await response.Content.ReadFromJsonAsync<List<SanPham>>();
                    if (sanPhams == null)
                    {
                        ViewBag.Error = "Dữ liệu trả về là null sau khi parse JSON.";
                        return View(new List<SanPham>());
                    }
                    return View(sanPhams);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Lỗi khi parse JSON: " + ex.Message;
                    return View(new List<SanPham>());
                }
            }
            else
            {
                ViewBag.Error = "Không thể gọi API, mã lỗi: " + response.StatusCode;
                return View(new List<SanPham>());
            }
        }

        // POST: Cập nhật số lượng theo kiểm kho
        [HttpPost]
        public async Task<IActionResult> CapNhatKiemKho([FromForm] Dictionary<int, int> soLuongKiemKho)
        {
            var client = _httpClientFactory.CreateClient("BanHangDienMayAPI");
            var response = await client.PostAsJsonAsync("api/Warehouse/kiemkho", soLuongKiemKho);

            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { message = "API lỗi: " + msg });
            }

            return Ok(new { message = "Cập nhật kiểm kho thành công!" });
        }

    }
}
