using System;
using System.Collections.Generic;

namespace BanHangDienMay.Models;

public partial class DanhMucSanPham
{
    public int MaDanhMuc { get; set; }

    public string TenDanhMuc { get; set; } = null!;

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
