namespace DeveloperPortal.Models.JiraIssues;

public class JiraIssue(string id, string key, Fields fields)
{
    public string Id { get; set; } = id;
    public string Key { get; set; } = key;
    public Fields Fields { get; set; } = fields;
}