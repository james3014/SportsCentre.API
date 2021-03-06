using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SportsCentre.API.Models
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateJoined { get; set; }
        public string MembershipType { get; set; }
        public DateTime MembershipExpiry { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }

    }
}