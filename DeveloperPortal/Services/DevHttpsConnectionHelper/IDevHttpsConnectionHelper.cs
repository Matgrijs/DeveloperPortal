namespace DeveloperPortal.Services.DevHttpsConnectionHelper;

public interface IDevHttpsConnectionHelper
{
    HttpClient HttpClient { get; }
    string DevServerRootUrl { get; }
    HttpMessageHandler GetPlatformMessageHandler();
}
