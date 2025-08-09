using Npgsql;

namespace ETL.Processor.Repository
{
    // TODO: ETL.Processor must not deal with the DB directly,
    // instead it should call another service, let's say ETL.Persistence
    public class PostgreRepository : IRepository
    {
        // TODO: connection must be get by some kind of config (local or remote)
        private readonly string connectionString;
        private readonly string sql;

        public PostgreRepository()
        {
            connectionString = "Host=localhost;Port=1234;Database=revenue_db;Username=postgres;Password=postgres";

            sql = @" 
                INSERT INTO users_revenue (user_id, revenue)
                VALUES (@user_id, @revenue)
                ON CONFLICT (user_id)
                DO UPDATE SET revenue = users_revenue.revenue + EXCLUDED.revenue;";
        }

        public void AddRevenue(string userId, int revenue)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("user_id", userId);
            cmd.Parameters.AddWithValue("revenue", revenue);
            cmd.ExecuteNonQuery();
        }
    }
}