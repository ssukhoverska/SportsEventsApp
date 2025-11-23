using SportsEventsApp.Models;
using System.Threading.Tasks;

namespace SportsEventsApp.Repositories
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<int> SellTicketsAsync(int buyerId, int ticketId, int qty, int userId);
        Task SoftDeleteAsync(int ticketId, int userId);
    }
}
