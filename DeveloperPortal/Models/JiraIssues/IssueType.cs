using Newtonsoft.Json;

namespace DeveloperPortal.Models.JiraIssues;

public class IssueType
{
    [JsonProperty("name")] public required string Name { get; set; }
}