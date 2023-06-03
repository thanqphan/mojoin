using System.ComponentModel.DataAnnotations;

namespace mojoin.ViewModel
{
    public class LoginViewModel
    {
        [Key]
        [MaxLength(100)]
        [Required(ErrorMessage = ("Vui lòng nhập SĐT"))]
        [Display(Name = "Email or SDT")]
        [RegularExpression(@"^(0|\+84)[3-9]\d{8}$", ErrorMessage = "Sai định dạng Số điện thoại")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(5, ErrorMessage = "Bạn cần nhập mật khẩu tối thiểu 5 ký tự")]
        public string Password { get; set; }
    }
}
