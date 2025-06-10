using System;
using System.Collections.Generic;

namespace BanHangDienMay.Models;

public partial class ChiTietDonHang
{
    public int MaChiTiet { get; set; }

    public int? MaHoaDon { get; set; }

    public int? MaSanPham { get; set; }

    public decimal GiaBan { get; set; }

    public int SoLuong { get; set; }

    public int? SoThangBaoHanh { get; set; }

    public virtual HoaDon? MaHoaDonNavigation { get; set; }

    public virtual SanPham? MaSanPhamNavigation { get; set; }
}
