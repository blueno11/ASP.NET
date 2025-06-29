using System;
using System.Collections.Generic;

namespace BanHangDienMay.Models;

public partial class NhapKho
{
    public int MaNhapKho { get; set; }

    public int? MaSanPham { get; set; }

    public int SoLuong { get; set; }

    public DateOnly NgayNhap { get; set; }

    public string? LyDo { get; set; }

    public int? MaNguoiDung { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }

    public virtual SanPham? MaSanPhamNavigation { get; set; }
}
