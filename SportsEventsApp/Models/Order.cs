namespace SportsEventsApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int BuyerUserId { get; set; }
        public int TicketId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderedAt { get; set; }
    }
}
