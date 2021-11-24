using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationLogic.Catalog.Users.Dto

{
    public class LoginRequest
    {
        [Display(Name = "UserName")]
        public String UserName { get; set; }
        public String Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
