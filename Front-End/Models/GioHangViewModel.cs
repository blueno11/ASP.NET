namespace Front_End.Models
{
    public class GioHangViewModel
    {
        public List<SanPham> DanhSachSanPham { get; set; } = new();
        public List<CategoryDto> DanhSachLoai { get; set; } = new();
        public int? LoaiDuocChon { get; set; }
        public string? TuKhoa { get; set; }

        public List<GioHangItem> GioHang { get; set; } = new();
        public List<KhuyenMai> DanhSachKhuyenMai { get; set; } = new();
    }
}
