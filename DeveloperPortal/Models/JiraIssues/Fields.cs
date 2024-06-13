using Newtonsoft.Json;

namespace DeveloperPortal.Models.JiraIssues;

public class Fields
{
    [JsonProperty("summary")] public required string Summary { get; set; }

    [JsonProperty("description")] public string? Description { get; set; }

    [JsonProperty("issuetype")] public required IssueType IssueType { get; set; }

    [JsonProperty("project")] public required Project Project { get; set; }
}