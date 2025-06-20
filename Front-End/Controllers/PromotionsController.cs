using Microsoft.AspNetCore.Mvc;

namespace Front_End.Controllers
{
    public class PromotionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            ViewBag.MaKhuyenMai = id;
            return View();
        }
    }
}
