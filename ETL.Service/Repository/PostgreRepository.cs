using Npgsql;
using ETL.Service.Repository;

// TODO: ETL.Service must not deal with the DB directly,
// instead it should call another service, let's say ETL.Persistence
public class PostgreRepository : IRepository
{
    // TODO: connectionString must be get by some kind of config (local or remote)
    private readonly string connectionString;
    private readonly string sql;

    public PostgreRepository()
    {
        connectionString = "Host=localhost;Port=1234;Database=revenue_db;Username=postgres;Password=postgres";
        sql = "SELECT revenue FROM users_revenue WHERE user_id = @user_id";
    }

    public async Task<int> GetRevenue(string userId)
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("user_id", userId);

        var result = await cmd.ExecuteScalarAsync();

        return result == null
            ? 0 // TODO: is it ok to return 0 if no revenue is found?
            : Convert.ToInt32(result);
    }
}