namespace SportsEventsApp.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int SportId { get; set; }
        public string? Description { get; set; }
        public int StatusId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int VenueId { get; set; }
        public int OrganizerUserId { get; set; }
    }
}
