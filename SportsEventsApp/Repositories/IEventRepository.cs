using SportsEventsApp.Models;
using System.Threading.Tasks;

namespace SportsEventsApp.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task SoftDeleteAsync(int eventId, int userId);
        Task RestoreAsync(int eventId, int userId);
    }
}
