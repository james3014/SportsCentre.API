using System;

namespace SportsCentre.API.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public DateTime ClassDate { get; set; }
        public int MaxAttendees { get; set; }
        public int TotalAttendees { get; set; }
        public Staff Attendant { get; set; }
        public Booking[] Bookings { get; set; }
    }
}