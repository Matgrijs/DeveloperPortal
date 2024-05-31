using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace  DeveloperPortal.Services;
    
public class JiraService
{
    private readonly HttpClient _httpClient;
    private readonly string _jiraBaseUrl = "https://developerportalavans.atlassian.net";

    public JiraService()
    {
        string email = "matthijs070403@gmail.com";
        string apiToken = "ATATT3xFfGF0_tj6u50DUWrjDU5bYYbS5FDcUgEOO4J4AjwcYuu8R8TIVxNblVjVI4cWwpk5HQH3Tyj3anL-IE_nF1j5LBB6Kik3HX7wWuzRJneGo3nT9vidE-WFpopmUoVxduDsBwY59c0-SDGnmXAGbXa4ObFY8lNGY6zOH-DBCYxhaTcDskA=E943B600";
        string credentials = $"{email}:{apiToken}";
        string encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
    }

    public async Task<List<JiraIssue>> GetIssuesByTypeAsync(string issueType)
    {
        var jqlQuery = $"issuetype={issueType}";
        var response = await _httpClient.GetAsync($"{_jiraBaseUrl}/rest/api/2/search?jql={jqlQuery}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var searchResults = JsonConvert.DeserializeObject<JiraSearchResults>(content);
        return searchResults.Issues;
    }

    public async Task<bool> CreateIssueAsync(JiraIssueCreateRequest request)
    {
        var jsonContent = JsonConvert.SerializeObject(request);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_jiraBaseUrl}/rest/api/2/issue", content);
        if (!response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Jira API response: {responseContent}");
        }
        return response.IsSuccessStatusCode;
    }

}

public class JiraIssue
{
    public string Id { get; set; }
    public string Key { get; set; }
    public Fields Fields { get; set; }
}

public class JiraIssueCreateRequest
{
    [JsonProperty("fields")]
    public Fields Fields { get; set; }
}

public class Fields
{
    [JsonProperty("summary")]
    public string Summary { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("issuetype")]
    public IssueType IssueType { get; set; }

    [JsonProperty("project")]
    public Project Project { get; set; }
}

public class IssueType
{
    [JsonProperty("name")]
    public string Name { get; set; }
}

public class Project
{
    [JsonProperty("key")]
    public string Key { get; set; }
}


public class JiraSearchResults
{
    [JsonProperty("issues")]
    public List<JiraIssue> Issues { get; set; }
}