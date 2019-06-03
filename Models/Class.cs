using System;

namespace SportsCentre.API.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public DateTime ClassDate { get; set; }
        public string ClassTime { get; set; }
        public string Facility { get; set; }
        public int MaxAttendees { get; set; }
        public int TotalAttendees { get; set; }
        public User User { get; set; }
        public double Cost { get; set; }
    }
}