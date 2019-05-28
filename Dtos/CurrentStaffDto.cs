using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsCentre.API.Dtos
{
    public class CurrentStaffDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public string Role { get; set; }
    }
}
