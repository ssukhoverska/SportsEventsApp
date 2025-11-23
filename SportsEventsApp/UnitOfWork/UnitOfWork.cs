using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SportsEventsApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlConnection _conn;
        private SqlTransaction? _tx;

        public IAthleteRepository Athletes { get; private set; }
        public IEventRepository Events { get; private set; }
        public ITicketRepository Tickets { get; private set; }

        public UnitOfWork(string connectionString)
        {
            _conn = new SqlConnection(connectionString);
            _conn.Open();
            _tx = _conn.BeginTransaction();

            // створюємо репозиторії з тією ж підключенням і транзакцією
            Athletes = new AthleteRepository(_conn, _tx);
            Events = new EventRepository(_conn, _tx);
            Tickets = new TicketRepository(_conn, _tx);
        }

        public async Task CommitAsync()
        {
            // commit and start a new transaction for subsequent operations (if needed)
            _tx?.Commit();
            _tx?.Dispose();
            _tx = _conn.BeginTransaction();

            // re-create repositories so they use new transaction
            Athletes = new AthleteRepository(_conn, _tx);
            Events = new EventRepository(_conn, _tx);
            Tickets = new TicketRepository(_conn, _tx);

            await Task.CompletedTask;
        }

        public void Rollback()
        {
            _tx?.Rollback();
            _tx?.Dispose();
            _tx = _conn.BeginTransaction();

            Athletes = new AthleteRepository(_conn, _tx);
            Events = new EventRepository(_conn, _tx);
            Tickets = new TicketRepository(_conn, _tx);
        }

        public void Dispose()
        {
            _tx?.Dispose();
            _conn?.Dispose();
        }
    }
}
