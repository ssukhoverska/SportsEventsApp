using SportsEventsApp.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SportsEventsApp.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly SqlConnection _conn;
        private readonly SqlTransaction? _tx;

        public TicketRepository(SqlConnection conn, SqlTransaction? tx)
        {
            _conn = conn;
            _tx = tx;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            var list = new List<Ticket>();
            using var cmd = new SqlCommand("SELECT id, event_id, name, description, price, quantity_total, quantity_sold FROM sem.tickets WHERE is_deleted=0", _conn, _tx);
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new Ticket
                {
                    Id = (int)rdr["id"],
                    EventId = (int)rdr["event_id"],
                    Name = rdr["name"]?.ToString(),
                    Description = rdr["description"]?.ToString(),
                    Price = (decimal)rdr["price"],
                    QuantityTotal = (int)rdr["quantity_total"],
                    QuantitySold = (int)rdr["quantity_sold"]
                });
            }
            return list;
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            using var cmd = new SqlCommand("SELECT id, event_id, name, description, price, quantity_total, quantity_sold FROM sem.tickets WHERE id=@id AND is_deleted=0", _conn, _tx);
            cmd.Parameters.AddWithValue("@id", id);
            using var rdr = await cmd.ExecuteReaderAsync();
            if (await rdr.ReadAsync())
            {
                return new Ticket
                {
                    Id = (int)rdr["id"],
                    EventId = (int)rdr["event_id"],
                    Name = rdr["name"]?.ToString(),
                    Description = rdr["description"]?.ToString(),
                    Price = (decimal)rdr["price"],
                    QuantityTotal = (int)rdr["quantity_total"],
                    QuantitySold = (int)rdr["quantity_sold"]
                };
            }
            return null;
        }

        public async Task<int> SellTicketsAsync(int buyerId, int ticketId, int qty, int userId)
        {
            using var cmd = new SqlCommand("sem.sell_tickets", _conn, _tx)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@buyer_id", buyerId);
            cmd.Parameters.AddWithValue("@ticket_id", ticketId);
            cmd.Parameters.AddWithValue("@qty", qty);
            cmd.Parameters.AddWithValue("@user_id", userId);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task SoftDeleteAsync(int ticketId, int userId)
        {
            using var cmd = new SqlCommand("sem.soft_delete_ticket", _conn, _tx) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ticket_id", ticketId);
            cmd.Parameters.AddWithValue("@user_id", userId);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
