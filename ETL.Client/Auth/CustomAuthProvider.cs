using System.Text;
using System.Text.Json;

namespace ETL.Client.Auth
{
    public class CustomAuthProvider : IAuthProvider
    {
        // TODO: AuthUrl and MediaType must be get by some kind of config (local or remote)
        private const string AuthUrl = "http://localhost:5090/auth";
        private const string MediaType = "application/json";

        private readonly HttpClient httpClient;
        private readonly StringContent authContent;

        public CustomAuthProvider()
        {
            httpClient = new()
            {
                Timeout = TimeSpan.FromMinutes(5)
            };

            authContent = CreateAuthContent();
        }

        private StringContent CreateAuthContent()
        {
            string credentials = "{\"user\":\"test\",\"password\":\"test\"}";
            return new StringContent(credentials, Encoding.UTF8, MediaType);
        }

        public async Task<string?> GetToken()
        {
            HttpResponseMessage authResponse = await httpClient.PostAsync(AuthUrl, authContent);
            string tokenJson = await authResponse.Content.ReadAsStringAsync();

            // TODO: token must be validated before use

            JsonDocument jsonDocument = JsonDocument.Parse(tokenJson);
            string? token = jsonDocument.RootElement.GetProperty("token").GetString();
            return token;
        }
    }
}