namespace Front_End.Models
{
    public class ChiTietDonHangDto
    {
        public int MaChiTiet { get; set; }

        public int? MaHoaDon { get; set; }

        public int MaSanPham { get;set; }
        public string TenSanPham { get; set; }

        public decimal GiaBan { get; set; }

        public int SoLuong { get; set; }

        public int? SoThangBaoHanh { get; set; }

        public decimal ThanhTien => GiaBan * SoLuong;
    }
}
