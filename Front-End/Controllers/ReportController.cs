using Front_End.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Front_End.Controllers
{
    public class ReportController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public ReportController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }


        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            if (HttpContext.Session.GetInt32("MaNguoiDung") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

    }
}