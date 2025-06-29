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

        [HttpGet("list")]
        public IActionResult GetAllUsers()
        {
            var users = _context.NguoiDungs.Select(u => new {
                Id = u.MaNguoiDung,
                Username = u.TenDangNhap,
                Email = "", // Nếu có trường email thì lấy, hiện tại model chưa có
                Role = u.VaiTro
            }).ToList();
            return Ok(users);
        }

        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] NguoiDung user)
        {
            if (string.IsNullOrWhiteSpace(user.TenDangNhap) || string.IsNullOrWhiteSpace(user.MatKhau) || string.IsNullOrWhiteSpace(user.VaiTro))
                return BadRequest("Vui lòng nhập đầy đủ thông tin!");
            if (_context.NguoiDungs.Any(u => u.TenDangNhap == user.TenDangNhap))
                return BadRequest("Tên đăng nhập đã tồn tại!");
            _context.NguoiDungs.Add(user);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] NguoiDung user)
        {
            var existing = _context.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == id);
            if (existing == null) return NotFound();
            if (!string.IsNullOrWhiteSpace(user.TenDangNhap)) existing.TenDangNhap = user.TenDangNhap;
            if (!string.IsNullOrWhiteSpace(user.VaiTro)) existing.VaiTro = user.VaiTro;
            if (!string.IsNullOrWhiteSpace(user.MatKhau)) existing.MatKhau = user.MatKhau;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == id);
            if (user == null) return NotFound();
            _context.NguoiDungs.Remove(user);
            _context.SaveChanges();
            return Ok();
        }
    }
} 