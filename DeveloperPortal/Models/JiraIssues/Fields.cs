using Newtonsoft.Json;

namespace DeveloperPortal.Models.JiraIssues;

public class Fields
{
    [JsonProperty("summary")] public string Summary { get; set; }

    [JsonProperty("description")] public string? Description { get; set; }

    [JsonProperty("issuetype")] public IssueType IssueType { get; set; }

    [JsonProperty("project")] public Project Project { get; set; }
}