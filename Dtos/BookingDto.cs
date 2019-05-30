using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsCentre.API.Dtos
{
    public class BookingDto
    {
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public string BookingTime { get; set; }
        public string Facility { get; set; }
        public string Requirements { get; set; }
    }
}
