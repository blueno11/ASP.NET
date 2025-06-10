using System;
using System.Collections.Generic;

namespace BanHangDienMay.Models;

public partial class YeuCauBaoTri
{
    public int MaBaoTri { get; set; }

    public int? MaSanPham { get; set; }

    public int? MaKhachHang { get; set; }

    public DateOnly NgayYeuCau { get; set; }

    public string? MoTaSuCo { get; set; }

    public string TrangThai { get; set; } = null!;

    public DateOnly? NgayXuLy { get; set; }

    public int? MaNguoiDung { get; set; }

    public virtual KhachHang? MaKhachHangNavigation { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }

    public virtual SanPham? MaSanPhamNavigation { get; set; }
}
