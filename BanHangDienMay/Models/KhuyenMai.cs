using System;
using System.Collections.Generic;

namespace BanHangDienMay.Models;

public partial class KhuyenMai
{
    public int MaKhuyenMai { get; set; }

    public string TenKhuyenMai { get; set; } = null!;

    public string? MoTa { get; set; }

    public decimal PhanTramGiam { get; set; }

    public DateOnly NgayBatDau { get; set; }

    public DateOnly NgayKetThuc { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
