using System.ComponentModel.DataAnnotations;

namespace SportsCentre.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required()]
        [EmailAddress(ErrorMessage = "Email Provided Is Not Valid")]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password Must Be At Least 8 Characters")]
        public string Password { get; set; }
    }
}