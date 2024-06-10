namespace DeveloperPortal.Services;

public class BaseHttpClientService
{
    public readonly HttpClient Client;
    
    [Obsolete("Obsolete")]
    public BaseHttpClientService(IHandlerService service)
    {
        var handler = service.GetPlatformMessageHandler();

        Client = handler != null ? new HttpClient(handler) : new HttpClient();

        var baseUrl = new Uri("https://localhost:7059");
        if (Device.RuntimePlatform == Device.Android)
        {
            baseUrl = new Uri("https://10.0.2.2:7059");
        }
        Client.BaseAddress = baseUrl;
    }
}