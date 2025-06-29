using Microsoft.AspNetCore.Mvc;

namespace Front_End.Controllers
{
    public class ActivitiesController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Mặc định sẽ trả về Views/Activities/Index.cshtml
        }
    }
} 