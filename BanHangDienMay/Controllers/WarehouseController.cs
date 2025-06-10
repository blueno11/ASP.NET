using Microsoft.AspNetCore.Mvc;
using BanHangDienMay.Models;
using Microsoft.EntityFrameworkCore;

namespace BanHangDienMay.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly QlbanHangDienMayContext _context;

        public WarehouseController(QlbanHangDienMayContext context)
        {
            _context = context;
        }

        // GET: api/Warehouse
        [HttpGet("sanpham")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<SanPham>>> GetSanPhams()
        {
            var sanPhams = await _context.SanPhams
                .Select(sp => new SanPham
                {
                    MaSanPham = sp.MaSanPham,
                    MaDanhMuc = sp.MaDanhMuc,
                    TenSanPham = sp.TenSanPham,
                    TrangThai = sp.TrangThai,
                    SoThangBaoHanh = sp.SoThangBaoHanh,
                    SoLuong = sp.SoLuong
                })
                .ToListAsync();

            return sanPhams;
        }

        // POST: api/Warehouse/capnhat
        [HttpPost("capnhat")]
        public async Task<IActionResult> CapNhat([FromBody] Dictionary<int, int> soLuongNhap)
        {
            try
            {
                foreach (var item in soLuongNhap)
                {
                    var sanPham = await _context.SanPhams.FindAsync(item.Key);
                    if (sanPham != null)
                    {
                        sanPham.SoLuong += item.Value;
                    }
                }
                await _context.SaveChangesAsync();
                return Ok("Cập nhật thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi cập nhật: " + ex.Message);
            }
        }
        [HttpPost("kiemkho")]
        public async Task<IActionResult> CapNhatKiemKho([FromBody] Dictionary<int, int> soLuongKiemKho)
        {
            try
            {
                foreach (var item in soLuongKiemKho)
                {
                    var sanPham = await _context.SanPhams.FindAsync(item.Key);
                    if (sanPham != null)
                    {
                        sanPham.SoLuong = item.Value; // Cập nhật số lượng mới
                    }
                }
                await _context.SaveChangesAsync();
                return Ok(new { message = "Cập nhật kiểm kho thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi cập nhật kiểm kho: " + ex.Message);
            }
        }


        // GET: api/Warehouse/test-connection
        [HttpGet("test-connection")]
        public IActionResult TestConnection()
        {
            try
            {
                _context.Database.OpenConnection();
                _context.Database.CloseConnection();
                return Ok("Kết nối CSDL thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi kết nối CSDL: " + ex.Message);
            }
        }
    }
}
