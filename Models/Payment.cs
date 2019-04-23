using System;

namespace SportsCentre.API.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public User PaidBy { get; set; }
    }
}