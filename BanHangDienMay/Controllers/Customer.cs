using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BanHangDienMay.Models;

namespace BanHangDienMay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly QlbanHangDienMayContext _context;

        public CustomerController(QlbanHangDienMayContext context)
        {
            _context = context;
        }

        // GET: api/customer/List
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<KhachHang>>> GetCustomers()
        {
            var customers = await _context.KhachHangs
                .Select(kh => new KhachHang
                {
                    MaKhachHang = kh.MaKhachHang,
                    TenKhachHang = kh.TenKhachHang,
                    SoDienThoai = kh.SoDienThoai,
                    Email = kh.Email,
                    DiaChi = kh.DiaChi,
                    NgayTao = kh.NgayTao
                })
                .ToListAsync();

            return customers;
        }

        // GET: api/customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KhachHang>> GetCustomer(int id)
        {
            var customer = await _context.KhachHangs
                .Where(kh => kh.MaKhachHang == id)
                .Select(kh => new KhachHang
                {
                    MaKhachHang = kh.MaKhachHang,
                    TenKhachHang = kh.TenKhachHang,
                    SoDienThoai = kh.SoDienThoai,
                    Email = kh.Email,
                    DiaChi = kh.DiaChi,
                    NgayTao = kh.NgayTao
                })
                .FirstOrDefaultAsync();

            if (customer == null)
            {
                return NotFound(new { message = $"Không tìm thấy khách hàng với ID {id}." });
            }

            return customer;
        }

        // POST: api/customer/Insert
        [HttpPost("Insert")]
        public async Task<ActionResult<KhachHang>> CreateCustomer(KhachHang customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dữ liệu đầu vào không hợp lệ." });
            }

            // Validate uniqueness of phone and email
            if (_context.KhachHangs.Any(k => k.SoDienThoai == customer.SoDienThoai))
            {
                return BadRequest(new { message = "Số điện thoại đã tồn tại." });
            }

            if (_context.KhachHangs.Any(k => k.Email == customer.Email))
            {
                return BadRequest(new { message = "Email đã tồn tại." });
            }

            _context.KhachHangs.Add(customer);
            await _context.SaveChangesAsync();

            // Áp dụng Projection cho dữ liệu trả về
            var createdCustomer = new KhachHang
            {
                MaKhachHang = customer.MaKhachHang,
                TenKhachHang = customer.TenKhachHang,
                SoDienThoai = customer.SoDienThoai,
                Email = customer.Email,
                DiaChi = customer.DiaChi,
                NgayTao = customer.NgayTao
            };

            return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.MaKhachHang }, createdCustomer);
        }

        // PUT: api/customer/Update/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, KhachHang customer)
        {
            if (id != customer.MaKhachHang)
            {
                return BadRequest(new { message = "Mã khách hàng không khớp." });
            }

            var existingCustomer = await _context.KhachHangs.FindAsync(id);
            if (existingCustomer == null)
            {
                return NotFound(new { message = $"Không tìm thấy khách hàng với ID {id}." });
            }

            // Validate uniqueness of phone and email (excluding current customer)
            if (_context.KhachHangs.Any(k => k.SoDienThoai == customer.SoDienThoai && k.MaKhachHang != id))
            {
                return BadRequest(new { message = "Số điện thoại đã tồn tại." });
            }

            if (_context.KhachHangs.Any(k => k.Email == customer.Email && k.MaKhachHang != id))
            {
                return BadRequest(new { message = "Email đã tồn tại." });
            }

            existingCustomer.TenKhachHang = customer.TenKhachHang;
            existingCustomer.SoDienThoai = customer.SoDienThoai;
            existingCustomer.Email = customer.Email;
            existingCustomer.DiaChi = customer.DiaChi;

            _context.Entry(existingCustomer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.KhachHangs.Any(e => e.MaKhachHang == id))
                {
                    return NotFound(new { message = $"Không tìm thấy khách hàng với ID {id}." });
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/customer/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.KhachHangs.FindAsync(id);
            if (customer == null)
            {
                return NotFound(new { message = $"Không tìm thấy khách hàng với ID {id}." });
            }

            // Kiểm tra xem khách hàng có hóa đơn hoặc yêu cầu bảo trì không
            if (_context.HoaDons.Any(h => h.MaKhachHang == id) || _context.YeuCauBaoTris.Any(b => b.MaKhachHang == id))
            {
                return BadRequest(new { message = "Không thể xóa khách hàng vì đã có hóa đơn hoặc yêu cầu bảo trì liên quan." });
            }

            _context.KhachHangs.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/customer/Statistics
        [HttpGet("Statistics")]
        public async Task<ActionResult<object>> GetCustomerStatistics()
        {
            var currentDate = DateTime.Now;
            var firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var firstDayOfLastMonth = firstDayOfCurrentMonth.AddMonths(-1);
            var lastDayOfLastMonth = firstDayOfCurrentMonth.AddDays(-1);

            // Tổng số khách hàng hiện tại
            var totalCustomers = await _context.KhachHangs.CountAsync();

            // Số khách hàng mới trong tháng này
            var newCustomersThisMonth = await _context.KhachHangs
                .Where(k => k.NgayTao >= firstDayOfCurrentMonth)
                .CountAsync();

            // Số khách hàng mới trong tháng trước
            var newCustomersLastMonth = await _context.KhachHangs
                .Where(k => k.NgayTao >= firstDayOfLastMonth && k.NgayTao <= lastDayOfLastMonth)
                .CountAsync();

            // Tính phần trăm tăng/giảm
            var growthPercentage = newCustomersLastMonth == 0
                ? 100
                : ((newCustomersThisMonth - newCustomersLastMonth) * 100.0 / newCustomersLastMonth);

            // Áp dụng Projection cho dữ liệu trả về
            return new
            {
                totalCustomers,
                newCustomersThisMonth,
                newCustomersLastMonth,
                growthPercentage = Math.Round(growthPercentage, 1)
            };
        }
    }
}