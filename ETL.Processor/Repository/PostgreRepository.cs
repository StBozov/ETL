using Npgsql;

namespace ETL.Processor.Repository
{
    public class PostgreRepository : IRepository
    {
        private readonly string connectionString;
        private readonly string sqlQuery;

        public PostgreRepository()
        {
            connectionString = "Host=localhost;Port=1234;Database=revenue_db;Username=postgres;Password=postgres";

            sqlQuery = @" 
                INSERT INTO users_revenue (user_id, revenue)
                VALUES (@user_id, @revenue)
                ON CONFLICT (user_id)
                DO UPDATE SET revenue = users_revenue.revenue + EXCLUDED.revenue;";
        }

        public void AddRevenue(string userId, int revenue)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(sqlQuery, conn);

            cmd.Parameters.AddWithValue("user_id", userId);
            cmd.Parameters.AddWithValue("revenue", revenue);
            cmd.ExecuteNonQuery();
        }
    }
}