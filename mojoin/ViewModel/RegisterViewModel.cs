﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace mojoin.ViewModel
{
	public class RegisterViewModel
	{
		[Key]
		public int CustomerId { get; set; }

		[Display(Name = "Họ")]
		[Required(ErrorMessage = "Vui lòng nhập Họ Tên")]
		public string LastName { get; set; }
        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Vui lòng nhập Họ Tên")]
        public string FirstName { get; set; }

        [MaxLength(11)]
		[Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
		[Display(Name = "Điện thoại")]
		[DataType(DataType.PhoneNumber)]
		[Remote(action: "ValidatePhone", controller: "Login")]
		public string Phone { get; set; }

        [MaxLength(150)]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "ValidateEmail", controller: "Accounts")]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
		[Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
		[MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
		public string Password { get; set; }

		[MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
		[Display(Name = "Nhập lại mật khẩu")]
		[Compare("Password", ErrorMessage = "Nhập lại mật khẩu không đúng")]
		public string ConfirmPassword { get; set; }
	}
}
