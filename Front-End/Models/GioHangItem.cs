public class GioHangItem
{ 
    public int MaSanPham { get; set; }
    public string TenSanPham { get; set; } = string.Empty;
    public decimal Gia { get; set; }
    public int SoLuong { get; set; }

    public int soThangBaoHanh { get; set; }
    public decimal ThanhTien => SoLuong * Gia;
}
