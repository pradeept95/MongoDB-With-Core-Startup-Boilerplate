using System;
using System.ComponentModel.DataAnnotations;

namespace KNN.NULLPrinter.Core.Dto.AppUser
{
    public class AppUsersDto
    { 
        public string Id { get; set; }

        [Required(ErrorMessage = "Firstname is Required")]
        public string FirstName { get; set; } 
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Lastname is Required")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "Email is Required")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")] 
        public string Email { get; set; } 

        [Required(ErrorMessage = "Username is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required")]
        public string ConfirmPassword { get; set; }

        public bool IsActive { get; set; }
        public bool IsConfirmed { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string FullName { get; set; }
    }
}
