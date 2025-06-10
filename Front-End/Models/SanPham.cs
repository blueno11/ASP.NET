namespace Front_End.Models
{
    public class SanPham
    {
        public int MaSanPham { get; set; }
        public int MaDanhMuc { get; set; }
        public string TenSanPham { get; set; } = string.Empty; // Giá trị mặc định
        public string TrangThai { get; set; } = "ConHang"; // Giá trị mặc định
        public int SoLuong { get; set; }
        public int SoThangBaoHanh { get; set; }
        public CategoryDto? MaDanhMucNavigation { get; set; } // Nullable để tránh lỗi khi API trả về null
    }
}