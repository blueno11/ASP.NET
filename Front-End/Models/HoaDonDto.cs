using System;

namespace Front_End.Models
{
    public class HoaDonDto
    {
        public int ma_hoa_don { get; set; }
        public int ma_khach_hang { get; set; }
        public DateTime ngay_dat { get; set; }
        public decimal tong_tien { get; set; }
        public int? ma_khuyen_mai { get; set; }
        public int ma_nguoi_dung { get; set; }
    }
}
