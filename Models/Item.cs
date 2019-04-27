namespace SportsCentre.API.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public double Cost { get; set; }
        public int StockLevel { get; set; }
    }
}