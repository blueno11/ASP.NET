using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using BanHangDienMay.Models;

namespace BanHangDienMay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly QlbanHangDienMayContext _context;

        public ReportController(QlbanHangDienMayContext context)
        {
            _context = context;
        }

        // GET: api/report/summary
        [HttpGet("summary")]
        public async Task<ActionResult<object>> GetSummaryReport(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                // Nếu không có ngày, mặc định lấy dữ liệu trong 30 ngày gần nhất
                endDate = endDate ?? DateTime.Now;
                startDate = startDate ?? endDate.Value.AddDays(-30);

                // Làm sạch thời gian để dễ so sánh
                var startDateTime = startDate.Value.Date; // 00:00:00
                var endDateTime = endDate.Value.Date.AddDays(1).AddTicks(-1); // 23:59:59.999...

                var customerCount = await _context.KhachHangs
                    .Where(k => k.MaKhachHang > 0)
                    .CountAsync();

                var invoiceCount = await _context.HoaDons
                    .Where(h => h.NgayDat >= startDateTime && h.NgayDat <= endDateTime)
                    .CountAsync();

                var totalRevenue = await _context.HoaDons
                    .Where(h => h.NgayDat >= startDateTime && h.NgayDat <= endDateTime)
                    .SumAsync(h => (decimal?)h.TongTien ?? 0);

                var productCount = await _context.SanPhams.CountAsync();

                var dailyRevenue = await _context.HoaDons
                    .Where(h => h.NgayDat >= startDateTime && h.NgayDat <= endDateTime)
                    .GroupBy(h => h.NgayDat.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Revenue = g.Sum(h => (decimal?)h.TongTien ?? 0)
                    })
                    .OrderBy(g => g.Date)
                    .ToListAsync();

                return new
                {
                    CustomerCount = customerCount,
                    InvoiceCount = invoiceCount,
                    TotalRevenue = totalRevenue,
                    ProductCount = productCount,
                    DailyRevenue = dailyRevenue
                };
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Lỗi khi lấy dữ liệu báo cáo: {ex.Message}" });
            }
        }
    }
}