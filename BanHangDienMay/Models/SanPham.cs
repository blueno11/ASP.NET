using System;
using System.Collections.Generic;

namespace BanHangDienMay.Models;

public partial class SanPham
{
    public int MaSanPham { get; set; }

    public int? MaDanhMuc { get; set; }

    public string TenSanPham { get; set; } = null!;

    public string TrangThai { get; set; } = null!;

    public int? SoThangBaoHanh { get; set; }

    public int SoLuong { get; set; }

    public string? LinkHinhAnh { get; set; }
    public decimal Gia {  get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual DanhMucSanPham? MaDanhMucNavigation { get; set; }

    public virtual ICollection<NhapKho> NhapKhos { get; set; } = new List<NhapKho>();

    public virtual ICollection<YeuCauBaoTri> YeuCauBaoTris { get; set; } = new List<YeuCauBaoTri>();

    public virtual ICollection<YeuCauDoiTra> YeuCauDoiTras { get; set; } = new List<YeuCauDoiTra>();
}
