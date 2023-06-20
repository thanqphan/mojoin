using System.ComponentModel.DataAnnotations;

namespace mojoin.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Key]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email.")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập địa chỉ email hợp lệ.")]
        public string Email { get; set; }
    }
}
