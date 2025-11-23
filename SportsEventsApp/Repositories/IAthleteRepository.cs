using SportsEventsApp.Models;
using System.Threading.Tasks;

namespace SportsEventsApp.Repositories
{
    public interface IAthleteRepository : IRepository<Athlete>
    {
        Task<int> UpsertAsync(Athlete athlete, int userId);
        Task SoftDeleteAsync(int athleteId, int userId);
    }
}
