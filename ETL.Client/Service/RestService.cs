using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ETL.Client.Events;

namespace ETL.Client.Service
{
    public class RestService : IService
    {
        // TODO: GetUrl, PostUrl, and MediaType must be get by some kind of config (local or remote)

        private const string MediaType = "application/json";
        private const string PostUrl = "http://localhost:5091/liveEvent";
        private const string GetUrl = "http://localhost:5091/userRevenue";

        private readonly HttpClient httpClient;

        public RestService(string token)
        {
            httpClient = new()
            {
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue("Bearer", token)
                },
                Timeout = TimeSpan.FromMinutes(5)
            };
        }

        public async Task<UserRevenue> GetUserRevenue(string userId)
        {
            string url = $"{GetUrl}/{userId}";
            HttpResponseMessage response = await httpClient.GetAsync(url);

            HttpStatusCode statusCode = response.StatusCode;
            if (statusCode != HttpStatusCode.OK)
            {
                string errorMessage = $"Failed to get user revenue for userId: {userId}, status code: {statusCode})";
                throw new HttpRequestException(errorMessage);
            }

            var userRevenue = await response.Content.ReadFromJsonAsync<UserRevenue>();
            if (userRevenue is null)
            {
                string errorMessage = $"Failed to deserialize UserRevenue for userId: {userId}";
                throw new InvalidOperationException(errorMessage);
            }

            return userRevenue;
        }

        public async Task<bool> PostLiveEvent(LiveEvent e)
        {
            string payload = JsonSerializer.Serialize(e);
            StringContent postContent = new(payload, Encoding.UTF8, MediaType);

            var response = await httpClient.PostAsync(PostUrl, postContent);

            HttpStatusCode statusCode = response.StatusCode;
            return statusCode == HttpStatusCode.OK;
        }
    }
}