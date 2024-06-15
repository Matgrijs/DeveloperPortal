using IdentityModel.OidcClient;

namespace DeveloperPortal.Services.Helpers;

public static class ServiceLocator
{
    public static OidcClient AuthClient { get; set; }
}