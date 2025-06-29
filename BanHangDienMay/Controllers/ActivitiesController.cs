using BanHangDienMay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangDienMay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly QlbanHangDienMayContext _context;
        public ActivitiesController(QlbanHangDienMayContext context)
        {
            _context = context;
        }

        // GET: api/activities/recent
        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentActivities(int top = 5)
        {
            // Lấy các hoạt động gần đây nhất từ nhiều bảng
            var recentOrders = await _context.HoaDons
                .OrderByDescending(h => h.NgayDat)
                .Take(top)
                .Select(h => new {
                    Type = "Đơn hàng mới",
                    Time = h.NgayDat,
                    Description = $"Đơn hàng #{h.MaHoaDon} - Tổng tiền: {h.TongTien:N0}₫"
                }).ToListAsync();

            var recentCustomers = await _context.KhachHangs
                .OrderByDescending(k => k.NgayTao)
                .Take(top)
                .Select(k => new {
                    Type = "Khách hàng mới",
                    Time = k.NgayTao ?? DateTime.MinValue,
                    Description = $"Khách hàng: {k.TenKhachHang}"
                }).ToListAsync();

            var recentWarehouses = await _context.NhapKhos
                .OrderByDescending(n => n.NgayNhap)
                .Take(top)
                .Select(n => new {
                    Type = "Nhập kho mới",
                    Time = n.NgayNhap.ToDateTime(TimeOnly.MinValue),
                    Description = $"Nhập {n.SoLuong} sản phẩm (Mã SP: {n.MaSanPham})"
                }).ToListAsync();

            var recentMaintenances = await _context.YeuCauBaoTris
                .OrderByDescending(y => y.NgayYeuCau)
                .Take(top)
                .Select(y => new {
                    Type = "Yêu cầu bảo trì",
                    Time = y.NgayYeuCau.ToDateTime(TimeOnly.MinValue),
                    Description = $"Bảo trì sản phẩm (Mã SP: {y.MaSanPham}) - {y.TrangThai}"
                }).ToListAsync();

            var recentReturns = await _context.YeuCauDoiTras
                .OrderByDescending(y => y.NgayYeuCau)
                .Take(top)
                .Select(y => new {
                    Type = "Yêu cầu đổi trả",
                    Time = y.NgayYeuCau.ToDateTime(TimeOnly.MinValue),
                    Description = $"Đổi trả sản phẩm (Mã SP: {y.MaSanPham}) - {y.TrangThai}"
                }).ToListAsync();

            // Gộp và sắp xếp theo thời gian giảm dần, lấy top N
            var allActivities = recentOrders
                .Concat(recentCustomers)
                .Concat(recentWarehouses)
                .Concat(recentMaintenances)
                .Concat(recentReturns)
                .OrderByDescending(a => a.Time)
                .Take(top)
                .ToList();

            return Ok(allActivities);
        }
    }
} 