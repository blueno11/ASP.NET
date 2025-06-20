using BanHangDienMay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Route("api/[controller]")][ApiController] public class CategoriesController : ControllerBase { private readonly QlbanHangDienMayContext _context;
    public CategoriesController(QlbanHangDienMayContext context)
    {
        _context = context;
    }

    // GET: api/categories
    [HttpGet("List")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _context.DanhMucSanPhams
            .Select(c => new CategoryDto
            {
                MaDanhMuc = c.MaDanhMuc,
                TenDanhMuc = c.TenDanhMuc
            })
            .ToListAsync();
        return Ok(categories);
    }

    // GET: api/categories/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await _context.DanhMucSanPhams
            .Where(c => c.MaDanhMuc == id)
            .Select(c => new CategoryDto
            {
                MaDanhMuc = c.MaDanhMuc,
                TenDanhMuc = c.TenDanhMuc
            })
            .FirstOrDefaultAsync();
        if (category == null) return NotFound("Danh mục không tồn tại.");
        return Ok(category);
    }

    // POST: api/categories
    [HttpPost("Insert")]
    public async Task<IActionResult> CreateCategory([FromBody] DanhMucSanPham category)
    {
        if (category == null || string.IsNullOrWhiteSpace(category.TenDanhMuc))
            return BadRequest("Tên danh mục không hợp lệ.");

        if (await _context.DanhMucSanPhams.AnyAsync(c => c.TenDanhMuc == category.TenDanhMuc))
            return BadRequest("Tên danh mục đã tồn tại.");

        _context.DanhMucSanPhams.Add(category);
        await _context.SaveChangesAsync();

        // Trả về CategoryDto để tránh vòng lặp tham chiếu
        var createdCategory = new CategoryDto
        {
            MaDanhMuc = category.MaDanhMuc,
            TenDanhMuc = category.TenDanhMuc
        };

        return CreatedAtAction(nameof(GetCategory), new { id = category.MaDanhMuc }, createdCategory);
    }

    // PUT: api/categories/5
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] DanhMucSanPham category)
    {
        if (id != category.MaDanhMuc || string.IsNullOrWhiteSpace(category.TenDanhMuc))
            return BadRequest("Dữ liệu không hợp lệ.");

        var existingCategory = await _context.DanhMucSanPhams.FindAsync(id);
        if (existingCategory == null) return NotFound("Danh mục không tồn tại.");

        if (await _context.DanhMucSanPhams.AnyAsync(c => c.TenDanhMuc == category.TenDanhMuc && c.MaDanhMuc != id))
            return BadRequest("Tên danh mục đã tồn tại.");

        existingCategory.TenDanhMuc = category.TenDanhMuc;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/categories/5
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.DanhMucSanPhams.FindAsync(id);
        if (category == null) return NotFound("Danh mục không tồn tại.");

        if (await _context.SanPhams.AnyAsync(p => p.MaDanhMuc == id))
            return BadRequest("Danh mục đang tồn tại sản phẩm.");

        _context.DanhMucSanPhams.Remove(category);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

public class CategoryDto { public int MaDanhMuc { get; set; } public string? TenDanhMuc { get; set; } }