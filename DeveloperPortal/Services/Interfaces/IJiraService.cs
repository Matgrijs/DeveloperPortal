using DeveloperPortal.Models.JiraIssues;

namespace DeveloperPortal.Services.Interfaces
{
    public interface IJiraService
    {
        Task<JiraSearchResults?> GetIssueAsync();
        Task<bool> CreateIssueAsync(CreateJiraIssueDto request);
    }
}