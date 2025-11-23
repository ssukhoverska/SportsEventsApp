using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsEventsApp.Repositories
{
    public interface IRepository<T>
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
