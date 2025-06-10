using System;
using System.Collections.Generic;

namespace BanHangDienMay.Models;

public partial class NguoiDung
{
    public int MaNguoiDung { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string VaiTro { get; set; } = null!;

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<NhapKho> NhapKhos { get; set; } = new List<NhapKho>();

    public virtual ICollection<YeuCauBaoTri> YeuCauBaoTris { get; set; } = new List<YeuCauBaoTri>();

    public virtual ICollection<YeuCauDoiTra> YeuCauDoiTras { get; set; } = new List<YeuCauDoiTra>();
}
