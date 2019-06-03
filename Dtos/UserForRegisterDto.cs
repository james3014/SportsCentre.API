using System;
using System.ComponentModel.DataAnnotations;

namespace SportsCentre.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required]
        public string Town { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        public DateTime DateRegistered { get; set; }

        public UserForRegisterDto()
        {
            DateRegistered = DateTime.Now;
        }
    }
}