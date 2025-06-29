using System;
using System.Collections.Generic;

namespace BanHangDienMay.Models;

public partial class HoaDon
{
    public int MaHoaDon { get; set; }

    public int? MaKhachHang { get; set; }

    public DateTime NgayDat { get; set; }

    public decimal TongTien { get; set; }

    public int? MaKhuyenMai { get; set; }

    public int MaNguoiDung { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual KhachHang? MaKhachHangNavigation { get; set; }

    public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }

    public virtual ICollection<YeuCauDoiTra> YeuCauDoiTras { get; set; } = new List<YeuCauDoiTra>();
}
