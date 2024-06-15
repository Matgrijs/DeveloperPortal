using Newtonsoft.Json;

namespace DeveloperPortal.Models.JiraIssues;

public class Project
{
    [JsonProperty("key")] public required string Key { get; set; }
}