using System;
using System.Collections.Generic;

namespace BanHangDienMay.Models;

public partial class KhachHang
{
    public int MaKhachHang { get; set; }

    public string TenKhachHang { get; set; } = null!;

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public string? DiaChi { get; set; }

    public DateTime NgayTao { get; set; }


    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<YeuCauBaoTri> YeuCauBaoTris { get; set; } = new List<YeuCauBaoTri>();
}
