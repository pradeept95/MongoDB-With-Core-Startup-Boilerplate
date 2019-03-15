using System.ComponentModel.DataAnnotations;

namespace Application.Core.Dto.AppUser
{
    public class ChangePasswordDto
    { 
        [Required]
        public string Id { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; } 

        [Required(ErrorMessage = "Password is Required")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangeMyPasswordDto
    { 
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string ConfirmPassword { get; set; }
    }

}
