namespace DeveloperPortal.Services;

public class AuthenticationService
{
    public static AuthenticationService Instance { get; } = new();

    public string UserName { get; private set; }
    public string Auth0Id { get; private set; }
    public string Token { get; private set; }

    public void Initialize(string userName, string auth0Id, string token)
    {
        UserName = userName;
        Auth0Id = auth0Id;
        Token = token;
    }
}