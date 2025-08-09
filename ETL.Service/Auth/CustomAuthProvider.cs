using System.Text;

namespace ETL.Service.Auth
{
    public class CustomAuthProvider : IAuthProvider
    {
        // TODO: AuthUrl and ValidateTokenEndpoint must be get by some kind of config (local or remote)
        private const string MediaType = "application/json";
        private const string ValidateTokenEndpoint = "http://localhost:5090/validate";

        private readonly HttpClient httpClient;

        public CustomAuthProvider()
        {
            httpClient = new()
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
        }

        public async Task<bool> IsValidToken(string token)
        {
            // TODO: do not validate the `token` on each and every request until it expires.
            var content = new StringContent($"{{\"token\":\"{token}\"}}", Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(ValidateTokenEndpoint, content);
            return response.IsSuccessStatusCode;
        }
    }
}