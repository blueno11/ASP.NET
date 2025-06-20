using BanHangDienMay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class PromotionsController : ControllerBase
{
    private readonly QlbanHangDienMayContext _context;

    public PromotionsController(QlbanHangDienMayContext context)
    {
        _context = context;
    }

    // DTO for Promotion
    public class PromotionDto
    {
        public int MaKhuyenMai { get; set; }
        public string TenKhuyenMai { get; set; } = null!;
        public string? MoTa { get; set; }
        public decimal PhanTramGiam { get; set; }
        public DateOnly NgayBatDau { get; set; }
        public DateOnly NgayKetThuc { get; set; }
    }

    // GET: api/promotions/List
    [HttpGet("List")]
    public async Task<IActionResult> GetAllPromotions()
    {
        var promotions = await _context.KhuyenMais
            .Select(static p => new PromotionDto
            {
                MaKhuyenMai = p.MaKhuyenMai,
                TenKhuyenMai = p.TenKhuyenMai,
                MoTa = p.MoTa,
                PhanTramGiam = p.PhanTramGiam,
                NgayBatDau =p.NgayBatDau,
                NgayKetThuc = p.NgayKetThuc
            })
            .ToListAsync();

        return Ok(promotions);
    }

    // GET: api/promotions/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPromotion(int id)
    {
        var promotion = await _context.KhuyenMais
            .Where(p => p.MaKhuyenMai == id)
            .Select(p => new PromotionDto
            {
                MaKhuyenMai = p.MaKhuyenMai,
                TenKhuyenMai = p.TenKhuyenMai,
                MoTa = p.MoTa,
                PhanTramGiam = p.PhanTramGiam,
                NgayBatDau = p.NgayBatDau,
                NgayKetThuc = p.NgayKetThuc
            })
            .FirstOrDefaultAsync();

        if (promotion == null) return NotFound("Chương trình khuyến mãi không tồn tại.");
        return Ok(promotion);
    }

    // POST: api/promotions/Insert
    [HttpPost("Insert")]
    public async Task<IActionResult> CreatePromotion([FromBody] KhuyenMai promotion)
    {
        if (promotion == null || string.IsNullOrWhiteSpace(promotion.TenKhuyenMai))
            return BadRequest("Tên chương trình khuyến mãi không hợp lệ.");

        if (promotion.PhanTramGiam < 0 || promotion.PhanTramGiam > 100)
            return BadRequest("Phần trăm giảm phải nằm trong khoảng 0 đến 100.");

        if (promotion.NgayKetThuc < promotion.NgayBatDau)
            return BadRequest("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");

        if (await _context.KhuyenMais.AnyAsync(p => p.TenKhuyenMai == promotion.TenKhuyenMai))
            return BadRequest("Tên chương trình khuyến mãi đã tồn tại.");

        _context.KhuyenMais.Add(promotion);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPromotion), new { id = promotion.MaKhuyenMai }, promotion);
    }

    // PUT: api/promotions/Update/{id}
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdatePromotion(int id, [FromBody] KhuyenMai promotion)
    {
        if (id != promotion.MaKhuyenMai || string.IsNullOrWhiteSpace(promotion.TenKhuyenMai))
            return BadRequest("Dữ liệu không hợp lệ.");

        var existingPromotion = await _context.KhuyenMais.FindAsync(id);
        if (existingPromotion == null) return NotFound("Chương trình khuyến mãi không tồn tại.");

        if (promotion.PhanTramGiam < 0 || promotion.PhanTramGiam > 100)
            return BadRequest("Phần trăm giảm phải nằm trong khoảng 0 đến 100.");

        if (promotion.NgayKetThuc < promotion.NgayBatDau)
            return BadRequest("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");

        if (await _context.KhuyenMais.AnyAsync(p => p.TenKhuyenMai == promotion.TenKhuyenMai && p.MaKhuyenMai != id))
            return BadRequest("Tên chương trình khuyến mãi đã tồn tại.");

        existingPromotion.TenKhuyenMai = promotion.TenKhuyenMai;
        existingPromotion.MoTa = promotion.MoTa;
        existingPromotion.PhanTramGiam = promotion.PhanTramGiam;
        existingPromotion.NgayBatDau = promotion.NgayBatDau;
        existingPromotion.NgayKetThuc = promotion.NgayKetThuc;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/promotions/Delete/{id}
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeletePromotion(int id)
    {
        var promotion = await _context.KhuyenMais.FindAsync(id);
        if (promotion == null) return NotFound("Chương trình khuyến mãi không tồn tại.");

        if (await _context.HoaDons.AnyAsync(h => h.MaKhuyenMai == id))
            return BadRequest("Không thể xóa chương trình khuyến mãi vì có hóa đơn liên quan.");

        _context.KhuyenMais.Remove(promotion);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // PUT: api/promotions/End/{id}
    [HttpPut("End/{id}")]
    public async Task<IActionResult> EndPromotion(int id)
    {
        var promotion = await _context.KhuyenMais.FindAsync(id);
        if (promotion == null) return NotFound("Chương trình khuyến mãi không tồn tại.");

        var today = DateOnly.FromDateTime(DateTime.Today);
        if (today < promotion.NgayBatDau)
            return BadRequest("Không thể kết thúc khuyến mãi vì ngày hiện tại nhỏ hơn ngày bắt đầu.");

        promotion.NgayKetThuc = today; // Kết thúc khuyến mãi vào ngày hiện tại
        await _context.SaveChangesAsync();
        return NoContent();
    }
}