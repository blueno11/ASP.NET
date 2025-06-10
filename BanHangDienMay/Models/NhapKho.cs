using System;
using System.Collections.Generic;

namespace BanHangDienMay.Models;

public partial class NhapKho
{
    public int MaNhapKho { get; set; }

    public required int MaSanPham { get; set; }

    public required int MaNguoiDung { get; set; }

    public required int SoLuong { get; set; }

    public required decimal DonGia { get; set; }

    public DateTime NgayNhap { get; set; }

    public string? LyDo { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;

}
