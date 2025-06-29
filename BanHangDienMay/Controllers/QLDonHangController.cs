using BanHangDienMay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanHangDienMay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QLDonHangController : Controller
    {
        private readonly QlbanHangDienMayContext _context;

        public QLDonHangController(QlbanHangDienMayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDonDto>>> GetHoaDons()
        {
            var hoaDons = await _context.HoaDons
                .Select(h => new HoaDonDto
                {
                    ma_hoa_don = h.MaHoaDon,
                    ngay_dat = h.NgayDat,
                    ma_khach_hang = h.MaKhachHang,
                    tong_tien = h.TongTien,
                    ma_khuyen_mai = h.MaKhuyenMai,
                    ma_nguoi_dung = h.MaNguoiDung,
                    MaKhachHangNavigation = h.MaKhachHangNavigation,
                })
                .ToListAsync();
            return Ok(hoaDons);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetHoaDon(int id)
        {
            HoaDonDto hoaDon = await _context.HoaDons
                    .Where(h => h.MaHoaDon == id)
                    .Select(h => new HoaDonDto
                    {
                        ma_hoa_don = h.MaHoaDon,
                        ngay_dat = h.NgayDat,
                        ma_khach_hang = h.MaKhachHang,
                        tong_tien = h.TongTien,
                        ma_khuyen_mai = h.MaKhuyenMai,
                        ma_nguoi_dung = h.MaNguoiDung,
                        MaKhachHangNavigation = h.MaKhachHangNavigation
                    })
                    .FirstOrDefaultAsync();

            return Ok(hoaDon);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] HoaDonDto hoaDonDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (hoaDonDto.ma_khach_hang == 0) hoaDonDto.ma_khach_hang = null;
            if (hoaDonDto.ma_khuyen_mai == 0) hoaDonDto.ma_khuyen_mai = null;
            // Tạo entity từ DTO
            var hoaDon = new HoaDon
            {
                MaKhachHang = hoaDonDto.ma_khach_hang,
                NgayDat = hoaDonDto.ngay_dat,
                TongTien = hoaDonDto.tong_tien,
                MaKhuyenMai = hoaDonDto.ma_khuyen_mai,
                MaNguoiDung = hoaDonDto.ma_nguoi_dung
            };

            Console.WriteLine(hoaDon.MaKhuyenMai);
            _context.HoaDons.Add(hoaDon);
            await _context.SaveChangesAsync();
            hoaDonDto.ma_hoa_don = hoaDon.MaHoaDon;
            // Trả về kết quả thành công (có thể trả về ID mới tạo)
            return CreatedAtAction(nameof(ChiTietDonHang), new { id = hoaDonDto.ma_hoa_don }, hoaDonDto);
        }

        [HttpPost("AddChiTiet")]
        public async Task<IActionResult> AddChiTiet([FromBody] List<ChiTietHoaDonDto> chiTietDonHang)
        {
            if (chiTietDonHang == null || !chiTietDonHang.Any())
                return BadRequest("Danh sách chi tiết đơn hàng trống.");

            foreach (var line in chiTietDonHang)
            {
                _context.ChiTietDonHangs.Add(new ChiTietDonHang
                {
                    MaHoaDon = line.MaHoaDon,
                    MaSanPham = line.MaSanPham,
                    GiaBan = line.GiaBan,
                    SoLuong = line.SoLuong,
                    SoThangBaoHanh = line.SoThangBaoHanh
                });
                var product = await _context.SanPhams.FirstOrDefaultAsync(p => p.MaSanPham == line.MaSanPham);
                if (product != null && product.SoLuong > 0)
                {
                    product.SoLuong -= line.SoLuong; // trừ 1 đơn vị
                    if (product.SoLuong == 0) {
                        product.TrangThai = "HetHang";
                    }
                    await _context.SaveChangesAsync();
                }
            }

            await _context.SaveChangesAsync();
            Console.WriteLine("----------------------------------------request");
            return Ok(new { message = "Thêm chi tiết đơn hàng thành công"});
        }


        [HttpGet]
        [Route("ChiTietDonHang/{id}")]
        public async Task<IActionResult> ChiTietDonHang(int id)
        {
            List<ChiTietHoaDonDto> chiTietHoaDon = await _context.ChiTietDonHangs
                .Where(p => p.MaHoaDon == id)
                .Select(p => new ChiTietHoaDonDto
                {
                    MaChiTiet= p.MaChiTiet,
                    TenSanPham = p.MaSanPhamNavigation.TenSanPham,
                    SoLuong = p.SoLuong,
                    GiaBan = p.GiaBan,
                })
                .ToListAsync();

            return Ok(chiTietHoaDon);
        }
        [HttpPost]
        [Route("tangDiem")]
        public async Task<IActionResult> TangDiemThuong([FromBody] TangDiemRequest request)
        {
            var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(c => c.MaKhachHang == request.MaKhachHang);

            khachHang.diemThuong += request.DiemTang;
            await _context.SaveChangesAsync();
            Console.WriteLine("----------------------------------------request");
            return Ok();
        }

        [HttpGet]
        [Route("SanPhamBan")]
        public async Task<IActionResult> LaySoSanPhamBan(DateTime? startDate, DateTime? endDate)
        {
            // Nếu không có ngày, mặc định lấy dữ liệu trong 30 ngày gần nhất
            endDate = endDate ?? DateTime.Now;
            startDate = startDate ?? endDate.Value.AddDays(-30);
            var chiTietDonhang = await _context.ChiTietDonHangs
                .Include(ct => ct.MaHoaDonNavigation)
                .Include(ct => ct.MaSanPhamNavigation)
                .Where(hd => hd.MaHoaDonNavigation.NgayDat >= startDate && hd.MaHoaDonNavigation.NgayDat <= endDate)
                .ToListAsync();

            var thongKeSanPham = chiTietDonhang
                .Where(ct => ct.MaSanPhamNavigation != null)
                .GroupBy(ct => ct.MaSanPhamNavigation.TenSanPham)
                .Select(g => new
                {
                    TenSanPham = g.Key,
                    TongSoLuong = g.Sum(ct => ct.SoLuong)
                })
                .ToList();

            return Ok(thongKeSanPham);
        }


    }

}

public class HoaDonDto
{
    public int ma_hoa_don { get; set; }
    public int? ma_khach_hang { get; set; }
    public DateTime ngay_dat { get; set; }
    public decimal tong_tien { get; set; }
    public int? ma_khuyen_mai { get; set; }
    public int ma_nguoi_dung { get; set; }
    public virtual KhachHang? MaKhachHangNavigation { get; set; }
}

public class ChiTietHoaDonDto
{
    public int MaChiTiet { get; set; }

    public int MaSanPham { get; set; }
    public int? MaHoaDon { get; set; }

    public string? TenSanPham { get; set; }

    public decimal GiaBan { get; set; }

    public int SoLuong { get; set; }

    public int? SoThangBaoHanh { get; set; }
}

public class TangDiemRequest
{
    public int DiemTang { get; set; }
    public int MaKhachHang { get; set; }
}