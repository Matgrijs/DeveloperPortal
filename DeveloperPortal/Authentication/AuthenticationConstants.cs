namespace DeveloperPortal.Authentication
{
    public static class AuthenticationConstants
    {
        public const string Auth0Audience = "https://developerportal.eu.auth0.com/api/v2/";

        public const string Auth0Domain = "developerportal.eu.auth0.com";

        public const string AppProtocolName = "myapp";

        public const string AppCallbackUrl = "callback";

        public const string ClientId = "dlRxoxG6hpTmFR6tGnNlhh8EX9bUd96d";

        public static readonly string[] Scopes = { "openid", "profile", "email"};
    }
}
