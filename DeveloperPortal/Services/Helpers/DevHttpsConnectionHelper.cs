using DeveloperPortal.Services.Interfaces;
using System.Net.Security;

namespace DeveloperPortal.Services.Helpers;

public class DevHttpsConnectionHelper : IHttpHandler
{
    private readonly Lazy<HttpClient> _lazyHttpClient;
    public int SslPort { get; }
    public string DevServerRootUrl { get; }

    public DevHttpsConnectionHelper(int sslPort)
    {
        SslPort = sslPort;
        DevServerRootUrl = FormattableString.Invariant($"https://{DevServerName}:{SslPort}");
        _lazyHttpClient = new Lazy<HttpClient>(() => new HttpClient(GetPlatformMessageHandler()!));
    }

    public HttpClient HttpClient => _lazyHttpClient.Value;

    public string DevServerName =>
#if WINDOWS
        "localhost";
#elif ANDROID
        "10.0.2.2";
#else
        throw new PlatformNotSupportedException("Only Windows and Android currently supported.");
#endif

    public HttpMessageHandler GetPlatformMessageHandler()
    {
#if WINDOWS
        return new HttpClientHandler();
#elif ANDROID
        var handler = new CustomAndroidMessageHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            if (cert != null && cert.Issuer.Equals("CN=localhost"))
                return true;
            return errors == SslPolicyErrors.None;
        };
        return handler;
#else
        throw new PlatformNotSupportedException("Only Windows and Android currently supported.");
#endif
    }

#if ANDROID
    internal sealed class CustomAndroidMessageHandler : Xamarin.Android.Net.AndroidMessageHandler
    {
        protected override Javax.Net.Ssl.IHostnameVerifier GetSSLHostnameVerifier(Javax.Net.Ssl.HttpsURLConnection connection)
        {
            return new CustomHostnameVerifier();
        }

        private sealed class CustomHostnameVerifier : Java.Lang.Object, Javax.Net.Ssl.IHostnameVerifier
        {
            public bool Verify(string? hostname, Javax.Net.Ssl.ISSLSession? session)
            {
                // Allows specific host names, depending on specific conditions
                return Javax.Net.Ssl.HttpsURLConnection.DefaultHostnameVerifier != null && (Javax.Net.Ssl.HttpsURLConnection.DefaultHostnameVerifier.Verify(hostname, session)
                    || (hostname == "10.0.2.2" && session?.PeerPrincipal?.Name == "CN=localhost"));
            }
        }
    }
#endif
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        return HttpClient.SendAsync(request);
    }

    public Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        return HttpClient.GetAsync(requestUri);
    }

    public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
    {
        return HttpClient.PostAsync(requestUri, content);
    }

    public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
    {
        return HttpClient.PutAsync(requestUri, content);
    }

    public Task<HttpResponseMessage> DeleteAsync(string requestUri)
    {
        return HttpClient.DeleteAsync(requestUri);
    }
}