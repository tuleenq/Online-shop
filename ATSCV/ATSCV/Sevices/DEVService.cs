namespace Environment.Services
{
    public class DEVService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DEVService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsUserLoggedIn()
        {
#if DEVELOPMENT
            return true; // Allow access without login in development.
#elif PRODUCTION || STAGING 
            var authToken = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");
            return !string.IsNullOrEmpty(authToken); 
//#elif STAGING
//            var authToken = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");
//            return !string.IsNullOrEmpty(authToken); // Enforce login in production.
#else
            throw new InvalidOperationException("Environment not supported.");
#endif


#pragma warning disable CS0162
            throw new InvalidOperationException("Environment not defined.");
#pragma warning restore CS0162
        }
    }
}
