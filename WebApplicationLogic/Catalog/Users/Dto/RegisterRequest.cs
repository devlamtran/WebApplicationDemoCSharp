using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationLogic.Catalog.Users.Dto

{
    public class RegisterRequest
    {
        //[Required(ErrorMessage = "Nhập họ")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "Nhập tên")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "Nhập ngày sinh")]
        [Display(Name = "Day Of Birth")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        //[Required(ErrorMessage = "Nhập email")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        //[Required(ErrorMessage = "Nhập số điện thoại")]
        [Display(Name = "PhoneNumber")]
        public String PhoneNumber { get; set; }

        //[Required(ErrorMessage = "Nhập tên người dùng")]
        [Display(Name = "UserName")]
        public String UserName { get; set; }

        //[Required(ErrorMessage = "Nhập mật khẩu")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        //[Required(ErrorMessage = "Nhập lại mật khẩu")]
        [DataType(DataType.Password)]
        public String ConfirmPassword { get; set; }



    }
}
