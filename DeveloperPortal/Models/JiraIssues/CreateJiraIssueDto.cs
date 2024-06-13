using Newtonsoft.Json;

namespace DeveloperPortal.Models.JiraIssues;

public class CreateJiraIssueDto
{
    [JsonProperty("fields")] public required Fields Fields { get; set; }
}