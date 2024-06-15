using System.Net.Http.Headers;
using System.Text;
using DeveloperPortal.Models.JiraIssues;
using DeveloperPortal.Services.Interfaces;
using Newtonsoft.Json;

namespace DeveloperPortal.Services;

public class JiraService: IJiraService
{
    private const string JiraBaseUrl = "https://developerportalavans.atlassian.net";
    private readonly HttpClient _httpClient;
    private JiraSearchResults? _jiraIssueList = new();
    private readonly string apiToken = Environment.GetEnvironmentVariable("JIRA_TOKEN");
    private readonly string email = "matthijs070403@gmail.com";

    public JiraService()
    {
        string credentials = $"{email}:{apiToken}";
        var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
    }

    public async Task<JiraSearchResults?> GetIssueAsync()
    {
        var jqlQuery = "issuetype=Bug";
        var response = await _httpClient.GetAsync($"{JiraBaseUrl}/rest/api/2/search?jql={jqlQuery}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var searchResults = JsonConvert.DeserializeObject<JiraSearchResults>(content);
            _jiraIssueList = searchResults;
        }

        return _jiraIssueList;
    }

    public async Task<bool> CreateIssueAsync(CreateJiraIssueDto request)
    {
        var jsonContent = JsonConvert.SerializeObject(request);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{JiraBaseUrl}/rest/api/2/issue", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        return response.IsSuccessStatusCode;
    }
}