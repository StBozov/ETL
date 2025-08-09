namespace ETL.Service.Auth
{
    public interface IAuthProvider
    {
        Task<bool> IsValidToken(string token);
    }
}