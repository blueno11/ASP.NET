using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Front_End.Models;

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
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
            {
                if (HttpContext.Session.GetInt32("MaNguoiDung") == null)
                {
                    TempData["ErrorMessage"] = "Bạn cần đăng nhập để truy cập chức năng này.";
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    TempData["ErrorMessage"] = "Chỉ quản trị viên mới được phép truy cập trang này.";
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
            {
                if (HttpContext.Session.GetInt32("MaNguoiDung") == null)
                {
                    TempData["ErrorMessage"] = "Bạn cần đăng nhập để truy cập chức năng này.";
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    TempData["ErrorMessage"] = "Chỉ quản trị viên mới được phép truy cập trang này.";
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(SanPham product, IFormFile? hinhAnh)
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
            {
                if (HttpContext.Session.GetInt32("MaNguoiDung") == null)
                    return RedirectToAction("Index", "Login");
                else return RedirectToAction("Index", "Home");
            }
            Console.WriteLine("Create Function đang chạy");
            var client = _clientFactory.CreateClient("BanHangDienMayAPI");
            var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(product.TenSanPham ?? ""), "TenSanPham");
            formData.Add(new StringContent(product.MaDanhMuc.ToString()), "MaDanhMuc");
            formData.Add(new StringContent(product.TrangThai ?? ""), "TrangThai");
            formData.Add(new StringContent(product.SoLuong.ToString()), "SoLuong");
            formData.Add(new StringContent(product.SoThangBaoHanh.ToString()), "SoThangBaoHanh");
            formData.Add(new StringContent(product.Gia.ToString()), "Gia");

            if (hinhAnh != null && hinhAnh.Length > 0)
            {
                formData.Add(new StreamContent(hinhAnh.OpenReadStream()), "hinhAnh", hinhAnh.FileName);
            }

            try
            {
                var response = await client.PostAsync("https://localhost:7156/api/products/Insert", formData);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "✅ Thêm sản phẩm thành công!";
                    return RedirectToAction("Index");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorContent);
                string errorMessage = "Lỗi tạo sản phẩm!";
                try
                {
                    // Nếu backend trả JSON
                    var errorObj = JsonSerializer.Deserialize<Dictionary<string, object>>(errorContent);
                    if (errorObj != null && errorObj.ContainsKey("message"))
                    {
                        errorMessage = $"❌ {errorObj["message"]}";
                    }
                    else
                    {
                        errorMessage = $"❌ {errorContent}";
                    }
                }
                catch
                {
                    // Không phải JSON thì hiển thị plain text
                    errorMessage = $"❌ {errorContent}";
                }
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = $"❌ Lỗi kết nối backend: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
            {
                if (HttpContext.Session.GetInt32("MaNguoiDung") == null)
                {
                    TempData["ErrorMessage"] = "Bạn cần đăng nhập để truy cập chức năng này.";
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    TempData["ErrorMessage"] = "Chỉ quản trị viên mới được phép truy cập trang này.";
                    return RedirectToAction("Index", "Home");
                }
            }
            var client = _clientFactory.CreateClient("BanHangDienMayAPI");
            try
            {
                var response = await client.GetAsync($"https://localhost:7156/api/products/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var product = JsonSerializer.Deserialize<SanPham>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (product == null || product.MaSanPham <= 0)
                        return NotFound("Sản phẩm không hợp lệ hoặc không tìm thấy.");

                    return View(product);
                }

                return NotFound($"Không tìm thấy sản phẩm với ID {id}. Mã trạng thái: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Lỗi kết nối đến backend: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SanPham product, IFormFile? hinhAnh)
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
            {
                if (HttpContext.Session.GetInt32("MaNguoiDung") == null)
                    return RedirectToAction("Index", "Login");
                else return RedirectToAction("Index", "Home");
            }
            var client = _clientFactory.CreateClient("BanHangDienMayAPI");
            var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(product.MaSanPham.ToString()), "MaSanPham");
            formData.Add(new StringContent(product.TenSanPham ?? ""), "TenSanPham");
            formData.Add(new StringContent(product.MaDanhMuc.ToString()), "MaDanhMuc");
            formData.Add(new StringContent(product.TrangThai ?? ""), "TrangThai");
            formData.Add(new StringContent(product.SoLuong.ToString()), "SoLuong");
            formData.Add(new StringContent(product.SoThangBaoHanh.ToString()), "SoThangBaoHanh");
            formData.Add(new StringContent(product.Gia.ToString()), "Gia");

            if (hinhAnh != null && hinhAnh.Length > 0)
            {
                formData.Add(new StreamContent(hinhAnh.OpenReadStream()), "hinhAnh", hinhAnh.FileName);
            }

            var response = await client.PutAsync($"https://localhost:7156/api/products/Update/{product.MaSanPham}", formData);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = $"Lỗi cập nhật sản phẩm: {errorContent}";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
            {
                if (HttpContext.Session.GetInt32("MaNguoiDung") == null)
                    return RedirectToAction("Index", "Login");
                else return RedirectToAction("Index", "Home");
            }
            var client = _clientFactory.CreateClient("BanHangDienMayAPI");

            try
            {
                var response = await client.GetAsync($"https://localhost:7156/api/products/delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
                    return RedirectToAction("Index");
                }

                TempData["ErrorMessage"] = $"Không tìm thấy sản phẩm với ID {id}. Mã trạng thái: {response.StatusCode}";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = $"Lỗi kết nối đến backend: {ex.Message}";
                return RedirectToAction("Index");
            }
            
        }
    }
}
