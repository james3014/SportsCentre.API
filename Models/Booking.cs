using System;

namespace SportsCentre.API.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string BookingEmail { get; set; }
        public string ContactNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public string BookingTime { get; set; }
        public User CreatedBy { get; set; }
        public string BookingType { get; set; }
        public Class Class { get; set; }
        public string FacilityType{ get; set; }
        public string Attendees { get; set; }
        public string Requirements { get; set; }
    }
}