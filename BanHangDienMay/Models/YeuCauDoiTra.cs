using System;
using System.Collections.Generic;

namespace BanHangDienMay.Models;

public partial class YeuCauDoiTra
{
    public int MaYeuCau { get; set; }

    public int? MaHoaDon { get; set; }

    public int? MaSanPham { get; set; }

    public DateOnly NgayYeuCau { get; set; }

    public string? LyDo { get; set; }

    public string TrangThai { get; set; } = null!;

    public DateOnly? NgayXuLy { get; set; }

    public int? MaNguoiDung { get; set; }

    public virtual HoaDon? MaHoaDonNavigation { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }

    public virtual SanPham? MaSanPhamNavigation { get; set; }
}
