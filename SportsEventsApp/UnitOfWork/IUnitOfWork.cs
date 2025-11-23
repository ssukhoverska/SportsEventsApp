using System;
using System.Threading.Tasks;

namespace SportsEventsApp.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAthleteRepository Athletes { get; }
        IEventRepository Events { get; }
        ITicketRepository Tickets { get; }

        Task CommitAsync();
        void Rollback();
    }
}
