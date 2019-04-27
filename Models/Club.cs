namespace SportsCentre.API.Models
{
    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Booking[] Bookings { get; set; }
        public Payment[] Payments { get; set; }
    }
}