using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DeveloperPortal.Models.JiraIssues;
using DeveloperPortal.Services.Interfaces;

namespace DeveloperPortal.ViewModels;

public partial class JiraIssueViewModel : BaseViewModel
{
    private readonly IJiraService _jiraService;

    [ObservableProperty] private ObservableCollection<JiraIssue> jiraIssues = new();

    public JiraIssueViewModel(IJiraService jiraService)
    {
        Title = "Jira Issues";
        _jiraService = jiraService;
    }

    public async Task GetJiraIssues()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var fetchedJiraIssues = await _jiraService.GetIssueAsync();

            if (JiraIssues.Count != 0)
                JiraIssues.Clear();

            if (fetchedJiraIssues?.Issues != null)
                foreach (var issue in fetchedJiraIssues.Issues)
                    JiraIssues.Add(issue);
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }
}