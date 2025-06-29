using Microsoft.AspNetCore.Mvc;

namespace Front_End.Controllers
{
    public class SettingsController : Controller
    {
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("VaiTro") == "Admin";
        }
        private IActionResult NotAllow()
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
        public IActionResult Index()
        {
            if (!IsAdmin()) return NotAllow();
            return View(); // Mặc định sẽ trả về Views/Settings/Index.cshtml
        }
        public IActionResult Create()
        {
            if (!IsAdmin()) return NotAllow();
            return View();
        }
        public IActionResult Edit(int id)
        {
            if (!IsAdmin()) return NotAllow();
            ViewBag.UserId = id;
            return View();
        }
    }
} 