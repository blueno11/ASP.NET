using Microsoft.AspNetCore.Mvc;
using BanHangDienMay.Models;
using System.Linq;

namespace BanHangDienMay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly QlbanHangDienMayContext _context;
        public AuthController(QlbanHangDienMayContext context)
        {
            _context = context;
        }

        public class LoginRequest
        {
            public string TenDangNhap { get; set; }
            public string MatKhau { get; set; }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.NguoiDungs.FirstOrDefault(u => u.TenDangNhap == request.TenDangNhap && u.MatKhau == request.MatKhau);
            if (user == null)
                return Unauthorized();
            // Có thể trả về thêm thông tin vai trò nếu cần
            return Ok(new { user.MaNguoiDung, user.TenDangNhap, user.VaiTro });
        }
    }
} 