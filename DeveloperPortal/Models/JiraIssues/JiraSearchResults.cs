using Newtonsoft.Json;

namespace DeveloperPortal.Models.JiraIssues;

public class JiraSearchResults
{
    [JsonProperty("issues")] public List<JiraIssue>? Issues { get; set; }
}