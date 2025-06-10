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

        public IActionResult Index()
        {
            return View();
        }
    }
}