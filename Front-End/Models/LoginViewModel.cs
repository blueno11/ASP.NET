using System.ComponentModel.DataAnnotations;

namespace Front_End.Models
{
    public class LoginViewModel
    {
        [Required]
        public string TenDangNhap { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }
    }
} 