using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationLogic.Catalog.Users.Dto

{
    public class LoginRequest
    {
       // [Required(ErrorMessage ="Nhập tên đăng nhập")]
        public String UserName { get; set; }

       // [Required(ErrorMessage = "Nhập mật khẩu")]
        public String Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
