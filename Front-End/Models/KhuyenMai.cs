namespace Front_End.Models
{
    public class KhuyenMai
    {
        public int MaKhuyenMai { get; set; }
        public string TenKhuyenMai { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public decimal PhanTramGiam { get; set; }
        public DateOnly NgayBatDau { get; set; }
        public DateOnly NgayKetThuc { get; set; }
    }
}