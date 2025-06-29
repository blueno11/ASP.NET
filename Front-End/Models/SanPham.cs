namespace Front_End.Models
{
    public class SanPham
    {
        public int MaSanPham { get; set; }

        public int MaDanhMuc { get; set; }

        public string TenSanPham { get; set; } = string.Empty;

        public string TrangThai { get; set; } = "ConHang";

        public int SoLuong { get; set; }

        public int SoThangBaoHanh { get; set; }

        public decimal Gia { get; set; }

        public string? Loai { get; set; } // Tạm giữ đúng tên như bạn đã có

        public CategoryDto? MaDanhMucNavigation { get; set; }

        /// <summary>
        /// Đường dẫn hoặc tên file ảnh sản phẩm
        /// </summary>
        public string? LinkHinhAnh { get; set; }
    }
}
