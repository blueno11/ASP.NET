using Microsoft.AspNetCore.Mvc;
using Front_End.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Front_End.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Gọi API xác thực đăng nhập ở backend
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:7156/api/auth/login", content); // Cập nhật lại URL nếu cần
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var userInfo = JsonSerializer.Deserialize<UserInfo>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    // Lưu vào session
                    HttpContext.Session.SetInt32("MaNguoiDung", userInfo.MaNguoiDung);
                    HttpContext.Session.SetString("TenDangNhap", userInfo.TenDangNhap);
                    HttpContext.Session.SetString("VaiTro", userInfo.VaiTro);
                    TempData["SuccessMessage"] = "Đăng nhập thành công!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                    ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
                    return View(model);
                }
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        // Thêm class phụ trợ
        private class UserInfo
        {
            public int MaNguoiDung { get; set; }
            public string TenDangNhap { get; set; }
            public string VaiTro { get; set; }
        }
    }
} 