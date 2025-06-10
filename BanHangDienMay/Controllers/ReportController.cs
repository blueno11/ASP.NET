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

                // Convert DateTime to DateOnly
                var startDateOnly = DateOnly.FromDateTime(startDate.Value);
                var endDateOnly = DateOnly.FromDateTime(endDate.Value);

                var customerCount = await _context.KhachHangs
                    .Where(k => k.MaKhachHang > 0)
                    .CountAsync();

                var invoiceCount = await _context.HoaDons
                    .Where(h => h.NgayDat >= startDateOnly && h.NgayDat <= endDateOnly)
                    .CountAsync();

                var totalRevenue = await _context.HoaDons
                    .Where(h => h.NgayDat >= startDateOnly && h.NgayDat <= endDateOnly)
                    .SumAsync(h => (decimal?)h.TongTien ?? 0);

                var dailyRevenue = await _context.HoaDons
                    .Where(h => h.NgayDat >= startDateOnly && h.NgayDat <= endDateOnly)
                    .GroupBy(h => h.NgayDat)
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