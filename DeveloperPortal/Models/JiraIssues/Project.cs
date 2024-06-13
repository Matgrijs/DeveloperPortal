using Newtonsoft.Json;

namespace DeveloperPortal.Models.JiraIssues;

public class Project
{
    [JsonProperty("key")] public string Key { get; set; }
}