using Microsoft.AspNetCore.Mvc;

namespace Front_End.Controllers
{
    public class PromotionsController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
            {
                if (HttpContext.Session.GetInt32("MaNguoiDung") == null)
                    return RedirectToAction("Index", "Login");
                else return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
            {
                if (HttpContext.Session.GetInt32("MaNguoiDung") == null)
                    return RedirectToAction("Index", "Login");
                else return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
            {
                if (HttpContext.Session.GetInt32("MaNguoiDung") == null)
                    return RedirectToAction("Index", "Login");
                else return RedirectToAction("Index", "Home");
            }
            ViewBag.MaKhuyenMai = id;
            return View();
        }
    }
}
