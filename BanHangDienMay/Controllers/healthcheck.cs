using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BanHangDienMay.Models;
using System.Threading.Tasks;

namespace BanHangDienMay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly QlbanHangDienMayContext _context;

        public HealthCheckController(QlbanHangDienMayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetHealthStatus()
        {
            var canConnect = await _context.Database.CanConnectAsync();

            if (canConnect)
                return Ok(new { status = "Healthy", dbConnected = true });

            return StatusCode(500, new { status = "Unhealthy", dbConnected = false });
        }
    }
}