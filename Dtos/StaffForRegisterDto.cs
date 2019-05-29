using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsCentre.API.Dtos
{
    public class StaffForRegisterDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email Provided Is Not Valid")]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password Must Be At Least 8 Characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "First Name Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Surname Required")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Address Required")]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Town or City Required")]
        public string Town { get; set; }

        [Required(ErrorMessage = "Post Code Required")]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Date of Birth Required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Role Required")]
        public string Role { get; set; }
    }
}
