using System.Text;
using System.Text.Json;

namespace ETL.Client.Auth
{
    public class CustomAuthProvider : IAuthProvider
    {
        private const string AuthUrl = "http://localhost:5090/auth";
        private const string MediaType = "application/json";

        private readonly HttpClient httpClient;
        private readonly StringContent authContent;

        public CustomAuthProvider()
        {
            httpClient = new();
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

            JsonDocument jsonDocument = JsonDocument.Parse(tokenJson); 
            string? token = jsonDocument.RootElement.GetProperty("token").GetString();
            return token;
        }
    }
}