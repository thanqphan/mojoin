
using System.ComponentModel.DataAnnotations;

namespace mojoin.ViewModel
{
    public class ProfileUserViewModel
    {
        [Key]
        public int UsserId { get; set; }
        [Required(ErrorMessage = "Vui lòng điền tên!")]
        [RegularExpression(@"^[a-zA-Z\sáàảãạăắằẳẵặâấầẩẫậéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵ]+$",
        ErrorMessage = "Sai định dạng tên.")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Vui lòng điền họ!")]
        [RegularExpression(@"^[a-zA-Z\sáàảãạăắằẳẵặâấầẩẫậéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵ]+$",
        ErrorMessage = "Sai định dạng họ tên.")]
        public string? LastName { get; set; }
        [MaxLength(150)]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Địa chỉ")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        public string? Sex { get; set; }
        public string? Avatar { get; set; }
    }
}
