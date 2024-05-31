using Auth0.AuthenticationApi;

namespace DeveloperPortal.Services
{
    public class Auth0ManagementService()
    {
        private const string ClientId = "LLW3LX93PdkwI2uEqLglcAd05R7uwoqs";
        private const string ClientSecret = "QzNcW9XrlekbRV6PDxipHhSFcbOUZI8dC0g6nEuai-KmQsoetwmY5NeFLwjQ-nfd";
        private const string Domain = "developerportal.eu.auth0.com";
        
        public async Task<string> GetManagementApiToken()
        {
            var client = new AuthenticationApiClient(new Uri($"https://{Domain}"));
            var tokenRequest = new Auth0.AuthenticationApi.Models.ClientCredentialsTokenRequest
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Audience = $"https://{Domain}/api/v2/"
            };

            var tokenResponse = await client.GetTokenAsync(tokenRequest);
            return tokenResponse.AccessToken;
        }
    }
}




