using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Front_End.Models;

namespace Front_End.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        int? maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
        if (!maNguoiDung.HasValue)
        {
           return RedirectToAction("Index","Login");
        }
        return View();
    }

    public IActionResult Privacy()
    {
        if (HttpContext.Session.GetInt32("MaNguoiDung") == null)
        {
            return RedirectToAction("Index", "Login");
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
