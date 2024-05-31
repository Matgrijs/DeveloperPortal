using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace  DeveloperPortal.Services;
public class SentryService
{
    private readonly HttpClient _httpClient;
    private readonly string _sentryApiToken;
    private readonly string _organization;
    private readonly string _project;

    public SentryService(string sentryApiToken, string organization, string project)
    {
        _httpClient = new HttpClient();
        _sentryApiToken = sentryApiToken;
        _organization = organization;
        _project = project;
    }

    public async Task<JArray> GetSentryErrorsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://sentry.io/api/0/projects/{_organization}/{_project}/issues/");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _sentryApiToken);

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JArray.Parse(content);
        }
        else
        {
            throw new Exception($"Failed to retrieve Sentry errors: {response.StatusCode}");
        }
    }
}