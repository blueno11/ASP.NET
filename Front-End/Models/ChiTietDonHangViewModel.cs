namespace Front_End.Models
{
    public class ChiTietDonHangViewModel
    {
        public int MaHoaDon { get; set; }
        public List<ChiTietDonHangDto> ChiTietDonHangs { get; set; } = new();
        public decimal TongTien { get; set; }

        public string? TenKhuyenMai {  get; set; }
        public decimal? PhanTramGiam { get; set; }

        public KhachHang? khachHang { get; set; }
    }
}
