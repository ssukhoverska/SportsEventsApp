namespace SportsEventsApp.Models
{
    public class Athlete
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Country { get; set; }
        public string? Bio { get; set; }
    }
}
