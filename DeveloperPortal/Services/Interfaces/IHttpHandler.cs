namespace DeveloperPortal.Services.Interfaces;

public interface IHttpHandler
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    Task<HttpResponseMessage> GetAsync(string requestUri);
    Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
    Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content);
    Task<HttpResponseMessage> DeleteAsync(string requestUri);
    HttpClient HttpClient { get; }
    string DevServerRootUrl { get; }
    HttpMessageHandler GetPlatformMessageHandler();
}