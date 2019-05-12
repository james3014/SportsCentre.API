using System;

namespace SportsCentre.API.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string BookingName { get; set; }
        public DateTime BookingDate { get; set; }
        public User CreatedBy { get; set; }
        public string BookingType { get; set; }
        public Payment PaymentDetail { get; set; }
    }
}