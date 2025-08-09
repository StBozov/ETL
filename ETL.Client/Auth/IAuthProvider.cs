namespace ETL.Client.Auth
{
    public interface IAuthProvider
    {
        Task<string?> GetToken();
    }
}