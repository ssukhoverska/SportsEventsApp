namespace SportsEventsApp.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityTotal { get; set; }
        public int QuantitySold { get; set; }
    }
}
