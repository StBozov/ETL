namespace ETL.Service.Auth
{
    // No need of thread safety for now
    public static class AuthProviderFactory
    {
        private static readonly IAuthProvider authProvider;

        static AuthProviderFactory()
        {
            authProvider = new CustomAuthProvider();
        }

        public static IAuthProvider Create()
        {
            return authProvider;
        }
    }
}