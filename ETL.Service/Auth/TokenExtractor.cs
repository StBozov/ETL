namespace ETL.Service.Auth
{
    public static class TokenExtractor
    {
        public static string Extract(HttpRequest req)
        {
            if (req.Headers.TryGetValue("Authorization", out var authHeader))
            {
                return authHeader.ToString().Replace("Bearer ", "");
            }
            return string.Empty;
        }
    }
}