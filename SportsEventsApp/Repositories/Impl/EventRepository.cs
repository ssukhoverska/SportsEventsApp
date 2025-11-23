using SportsEventsApp.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SportsEventsApp.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly SqlConnection _conn;
        private readonly SqlTransaction? _tx;

        public EventRepository(SqlConnection conn, SqlTransaction? tx)
        {
            _conn = conn;
            _tx = tx;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            var list = new List<Event>();
            using var cmd = new SqlCommand("SELECT id, title, sport_id, description, status_id, start_date, end_date, venue_id, organizer_user_id FROM sem.events WHERE is_deleted=0", _conn, _tx);
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new Event
                {
                    Id = (int)rdr["id"],
                    Title = rdr["title"]?.ToString(),
                    SportId = (int)rdr["sport_id"],
                    Description = rdr["description"]?.ToString(),
                    StatusId = (int)rdr["status_id"],
                    StartDate = rdr["start_date"] as DateTime?,
                    EndDate = rdr["end_date"] as DateTime?,
                    VenueId = (int)rdr["venue_id"],
                    OrganizerUserId = (int)rdr["organizer_user_id"]
                });
            }

            return list;
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            using var cmd = new SqlCommand("SELECT id, title, sport_id, description, status_id, start_date, end_date, venue_id, organizer_user_id FROM sem.events WHERE id=@id AND is_deleted=0", _conn, _tx);
            cmd.Parameters.AddWithValue("@id", id);
            using var rdr = await cmd.ExecuteReaderAsync();
            if (await rdr.ReadAsync())
            {
                return new Event
                {
                    Id = (int)rdr["id"],
                    Title = rdr["title"]?.ToString(),
                    SportId = (int)rdr["sport_id"],
                    Description = rdr["description"]?.ToString(),
                    StatusId = (int)rdr["status_id"],
                    StartDate = rdr["start_date"] as DateTime?,
                    EndDate = rdr["end_date"] as DateTime?,
                    VenueId = (int)rdr["venue_id"],
                    OrganizerUserId = (int)rdr["organizer_user_id"]
                };
            }
            return null;
        }

        public async Task SoftDeleteAsync(int eventId, int userId)
        {
            using var cmd = new SqlCommand("sem.soft_delete_event", _conn, _tx) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@event_id", eventId);
            cmd.Parameters.AddWithValue("@user_id", userId);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task RestoreAsync(int eventId, int userId)
        {
            using var cmd = new SqlCommand("sem.restore_event", _conn, _tx) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@event_id", eventId);
            cmd.Parameters.AddWithValue("@user_id", userId);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
