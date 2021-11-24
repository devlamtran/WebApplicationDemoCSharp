using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationLogic.Catalog.Users.Dto

{
    public class RegisterRequest
    {
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Day Of Birth")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Display(Name = "PhoneNumber")]
        public String PhoneNumber { get; set; }

        [Display(Name = "UserName")]
        public String UserName { get; set; }

        
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [DataType(DataType.Password)]
        public String ConfirmPassword { get; set; }



    }
}
