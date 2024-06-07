using Newtonsoft.Json;

namespace DeveloperPortal.Models.JiraIssues;

public class IssueType
{
    [JsonProperty("name")] public string Name { get; set; }
}