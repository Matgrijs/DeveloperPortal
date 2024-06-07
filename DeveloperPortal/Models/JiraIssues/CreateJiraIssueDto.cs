using Newtonsoft.Json;

namespace DeveloperPortal.Models.JiraIssues;

public class CreateJiraIssueDto
{
    [JsonProperty("fields")] public Fields Fields { get; set; }
}