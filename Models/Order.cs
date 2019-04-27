using System;

namespace SportsCentre.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public Item[] Items{ get; set; }
        public double Total { get; set; }
    }
}