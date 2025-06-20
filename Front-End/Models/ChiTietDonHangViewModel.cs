namespace Front_End.Models
{
    public class ChiTietDonHangViewModel
    {
        public int MaHoaDon { get; set; }
        public List<ChiTietDonHangDto> ChiTietDonHangs { get; set; } = new();
        public decimal TongTien => ChiTietDonHangs.Sum(c => c.ThanhTien);
    }
}
