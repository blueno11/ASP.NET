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
                    ma_khach_hang = h.MaKhachHang ?? 0,
                    tong_tien = h.TongTien,
                    ma_khuyen_mai = h.MaKhuyenMai,
                    ma_nguoi_dung = h.MaNguoiDung ?? 0
                })
                .ToListAsync();
            return Ok(hoaDons);
        }


        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] HoaDonDto hoaDonDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Tạo entity từ DTO
            var hoaDon = new HoaDon
            {
                MaKhachHang = hoaDonDto.ma_khach_hang,
                NgayDat = hoaDonDto.ngay_dat,
                TongTien = hoaDonDto.tong_tien,
                MaKhuyenMai = hoaDonDto.ma_khuyen_mai,
                MaNguoiDung = hoaDonDto.ma_nguoi_dung
            };

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
                
            }

            await _context.SaveChangesAsync();

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


    }

}

public class HoaDonDto
{
    public int ma_hoa_don { get; set; }
    public int ma_khach_hang { get; set; }
    public DateOnly ngay_dat { get; set; }
    public decimal tong_tien { get; set; }
    public int? ma_khuyen_mai { get; set; }
    public int ma_nguoi_dung { get; set; }
}

public class ChiTietHoaDonDto
{
    public int MaChiTiet { get; set; }
    public int MaSanPham { get; set; }
    public int MaHoaDon { get; set; }
    public string TenSanPham { get; set; } = string.Empty;
    public decimal GiaBan { get; set; }
    public int SoLuong { get; set; }
    public int? SoThangBaoHanh { get; set; }
}