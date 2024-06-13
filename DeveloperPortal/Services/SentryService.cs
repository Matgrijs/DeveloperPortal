using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace DeveloperPortal.Services;

public class SentryService
{
    private readonly HttpClient _httpClient;

    public SentryService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<JArray> GetSentryErrors()
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            "https://sentry.io/api/0/projects/developerportalavans/dotnet-maui/issues/");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer",
            "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");

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JArray.Parse(content);
        }

        throw new Exception($"Failed to retrieve Sentry errors: {response.StatusCode}");
    }
}