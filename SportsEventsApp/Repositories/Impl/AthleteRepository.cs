using SportsEventsApp.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace SportsEventsApp.Repositories
{
    public class AthleteRepository : IAthleteRepository
    {
        private readonly SqlConnection _conn;
        private readonly SqlTransaction? _tx;

        public AthleteRepository(SqlConnection conn, SqlTransaction? tx)
        {
            _conn = conn;
            _tx = tx;
        }

        public async Task<IEnumerable<Athlete>> GetAllAsync()
        {
            var list = new List<Athlete>();
            using var cmd = new SqlCommand("SELECT id, first_name, last_name, dob, country, bio FROM sem.athletes WHERE is_deleted=0", _conn, _tx);
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new Athlete
                {
                    Id = (int)rdr["id"],
                    FirstName = rdr["first_name"]?.ToString(),
                    LastName = rdr["last_name"]?.ToString(),
                    Dob = rdr["dob"] as DateTime?,
                    Country = rdr["country"]?.ToString(),
                    Bio = rdr["bio"]?.ToString()
                });
            }

            return list;
        }

        public async Task<Athlete?> GetByIdAsync(int id)
        {
            using var cmd = new SqlCommand("SELECT id, first_name, last_name, dob, country, bio FROM sem.athletes WHERE id=@id AND is_deleted=0", _conn, _tx);
            cmd.Parameters.AddWithValue("@id", id);
            using var rdr = await cmd.ExecuteReaderAsync();
            if (await rdr.ReadAsync())
            {
                return new Athlete
                {
                    Id = (int)rdr["id"],
                    FirstName = rdr["first_name"]?.ToString(),
                    LastName = rdr["last_name"]?.ToString(),
                    Dob = rdr["dob"] as DateTime?,
                    Country = rdr["country"]?.ToString(),
                    Bio = rdr["bio"]?.ToString()
                };
            }
            return null;
        }

        public async Task<int> UpsertAsync(Athlete athlete, int userId)
        {
            using var cmd = new SqlCommand("sem.upsert_athlete", _conn, _tx)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@id", athlete.Id == 0 ? (object?)DBNull.Value : athlete.Id);
            cmd.Parameters.AddWithValue("@first", (object?)athlete.FirstName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@last", (object?)athlete.LastName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@dob", (object?)athlete.Dob ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@country", (object?)athlete.Country ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@bio", (object?)athlete.Bio ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@user_id", userId);

            var result = await cmd.ExecuteScalarAsync();
            // stored proc returns SCOPE_IDENTITY() as numeric -> decimal
            return Convert.ToInt32(result);
        }

        public async Task SoftDeleteAsync(int athleteId, int userId)
        {
            using var cmd = new SqlCommand("sem.soft_delete_athlete", _conn, _tx)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@athlete_id", athleteId);
            cmd.Parameters.AddWithValue("@user_id", userId);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
