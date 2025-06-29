using BanHangDienMay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly QlbanHangDienMayContext _context;
    private readonly IWebHostEnvironment _env;

    public ProductsController(QlbanHangDienMayContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    // GET: api/products
    [HttpGet("List")]
    public async Task<IActionResult> GetAllProducts(int? maDanhMuc)
    {
        var query = _context.SanPhams.AsQueryable();

        // Lọc theo danh mục nếu có
        if (maDanhMuc.HasValue)
        {
            query = query.Where(p => p.MaDanhMuc == maDanhMuc.Value);
        }

        var products = await query
            .Select(p => new ProductDto
            {
                MaSanPham = p.MaSanPham,
                MaDanhMuc = p.MaDanhMuc,
                TenSanPham = p.TenSanPham,
                TrangThai = p.TrangThai,
                SoThangBaoHanh = p.SoThangBaoHanh,
                SoLuong = p.SoLuong,
                Gia = p.Gia,
                MaDanhMucNavigation = p.MaDanhMucNavigation != null ? new CategoryDto
                {
                    MaDanhMuc = p.MaDanhMucNavigation.MaDanhMuc,
                    TenDanhMuc = p.MaDanhMucNavigation.TenDanhMuc
                } : null,
                LinkHinhAnh = p.LinkHinhAnh
            })
            .ToListAsync();

        return Ok(products);
    }



    // GET: api/products/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _context.SanPhams
            .Where(p => p.MaSanPham == id)
            .Select(p => new ProductDto
            {
                MaSanPham = p.MaSanPham,
                MaDanhMuc = p.MaDanhMuc,
                TenSanPham = p.TenSanPham,
                TrangThai = p.TrangThai,
                SoThangBaoHanh = p.SoThangBaoHanh,
                SoLuong = p.SoLuong,
                Gia = p.Gia,
                LinkHinhAnh = p.LinkHinhAnh
            })
            .FirstOrDefaultAsync();
        if (product == null) return NotFound("Sản phẩm không tồn tại.");
        return Ok(product);
    }

    [HttpPost("Insert")]
    public async Task<IActionResult> CreateProduct([FromForm] SanPham product, IFormFile? hinhAnh)
    {

        

        if (string.IsNullOrWhiteSpace(product.TenSanPham))
            return BadRequest("Tên sản phẩm không hợp lệ.");

        if (!await _context.DanhMucSanPhams.AnyAsync(c => c.MaDanhMuc == product.MaDanhMuc))
            return BadRequest("Danh mục không tồn tại.");

        if (product.TrangThai != "ConHang" && product.TrangThai != "HetHang" && product.TrangThai != "NgungKinhDoanh")
            return BadRequest("Trạng thái không hợp lệ.");

        if (product.SoLuong < 0 || product.SoThangBaoHanh < 0)
            return BadRequest("Số lượng và số tháng bảo hành phải không âm.");

        if (hinhAnh != null && hinhAnh.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var fileName = Path.GetFileName(hinhAnh.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await hinhAnh.CopyToAsync(stream);
            }

            // Trả link đầy đủ bao gồm domain + path
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            product.LinkHinhAnh = $"{baseUrl}/uploads/{fileName}";
        }


        _context.SanPhams.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.MaSanPham }, product);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromForm] SanPham product, IFormFile? hinhAnh)
    {
        if (id != product.MaSanPham)
            return BadRequest("ID không khớp.");

        var existingProduct = await _context.SanPhams.FindAsync(id);
        if (existingProduct == null)
            return NotFound("Sản phẩm không tồn tại.");

        if (!await _context.DanhMucSanPhams.AnyAsync(c => c.MaDanhMuc == product.MaDanhMuc))
            return BadRequest("Danh mục không tồn tại.");

        if (product.TrangThai != "ConHang" && product.TrangThai != "HetHang" && product.TrangThai != "NgungKinhDoanh")
            return BadRequest("Trạng thái không hợp lệ.");

        if (product.SoLuong < 0 || product.SoThangBaoHanh < 0)
            return BadRequest("Số lượng và số tháng bảo hành phải không âm.");

        existingProduct.TenSanPham = product.TenSanPham;
        existingProduct.MaDanhMuc = product.MaDanhMuc;
        existingProduct.TrangThai = product.TrangThai;
        existingProduct.SoLuong = product.SoLuong;
        existingProduct.SoThangBaoHanh = product.SoThangBaoHanh;
        existingProduct.Gia = product.Gia;

        if (hinhAnh != null && hinhAnh.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var fileName = Path.GetFileName(hinhAnh.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await hinhAnh.CopyToAsync(stream);
            }

            // Trả link đầy đủ bao gồm domain + path
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            existingProduct.LinkHinhAnh = $"{baseUrl}/uploads/{fileName}";
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/products/5
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.SanPhams.FindAsync(id);
        if (product == null) return NotFound("Sản phẩm không tồn tại.");

        // Kiểm tra sản phẩm có liên kết với hóa đơn, đổi trả, bảo trì không
        if (await _context.YeuCauDoiTras.AnyAsync(r => r.MaSanPham == id) ||
            await _context.YeuCauBaoTris.AnyAsync(m => m.MaSanPham == id))
            return BadRequest("Không thể xóa sản phẩm vì có dữ liệu liên quan.");

        _context.SanPhams.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("best-selling")]
    public async Task<IActionResult> GetBestSellingProducts(int top = 5)
    {
        var bestSelling = await _context.ChiTietDonHangs
            .Where(ct => ct.MaSanPham != null)
            .GroupBy(ct => ct.MaSanPham)
            .Select(g => new {
                MaSanPham = g.Key.Value,
                SoLuongBan = g.Sum(x => x.SoLuong)
            })
            .OrderByDescending(g => g.SoLuongBan)
            .Take(top)
            .ToListAsync();

        var productIds = bestSelling.Select(x => x.MaSanPham).ToList();
        var products = await _context.SanPhams
            .Where(p => productIds.Contains(p.MaSanPham))
            .ToListAsync();

        var result = bestSelling.Select(x => {
            var p = products.FirstOrDefault(p => p.MaSanPham == x.MaSanPham);
            return new
            {
                MaSanPham = p?.MaSanPham,
                TenSanPham = p?.TenSanPham,
                SoLuongBan = x.SoLuongBan,
                Gia = p?.Gia,
                LinkHinhAnh = p?.LinkHinhAnh
            };
        });

        return Ok(result);
    }
}

public class ProductDto()
{
    public int MaSanPham { get; set; }

    public int? MaDanhMuc { get; set; }

    public string TenSanPham { get; set; } = null!;

    public string TrangThai { get; set; } = null!;

    public int? SoThangBaoHanh { get; set; }

    public int SoLuong { get; set; }

    public string? LinkHinhAnh { get; set; }
    public decimal Gia { get; set; }

    public CategoryDto? MaDanhMucNavigation { get; set; } // Thêm thông tin danh mục
}