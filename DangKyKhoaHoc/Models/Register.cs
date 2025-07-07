using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
namespace DangKyKhoaHoc.Models
{
    public class Register
    {
        [Required(ErrorMessage = "Họ tên không được bỏ trống")]
        public string FullName { get; set; }   

        [Required(ErrorMessage = "Email không được bỏ trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Nhập lại mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; } = "student";//mặc định là student
    }
}
