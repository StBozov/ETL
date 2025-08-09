using System.Text;

namespace ETL.Service.Auth
{
    public class CustomAuthProvider : IAuthProvider
    {
        private const string MediaType = "application/json";
        private const string ValidateEndpoint = "http://localhost:5090/validate";

        private readonly HttpClient httpClient;

        public CustomAuthProvider()
        {
            httpClient = new HttpClient();
        }

        public async Task<bool> IsValidToken(string token)
        {
            // TODO: do not validate the `token` on each and every request in a real life scenario.
            var content = new StringContent($"{{\"token\":\"{token}\"}}", Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(ValidateEndpoint, content);
            return response.IsSuccessStatusCode;
        }
    }
}