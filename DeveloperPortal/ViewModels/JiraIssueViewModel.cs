using System.Collections.ObjectModel;
using System.Diagnostics;
using DeveloperPortal.Models.JiraIssues;
using DeveloperPortal.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DeveloperPortal.ViewModels
{
    public partial class JiraIssueViewModel : BaseViewModel
    {
        private readonly JiraService _jiraService;

        [ObservableProperty]
        private ObservableCollection<JiraIssue> jiraIssues = new();

        public JiraIssueViewModel(JiraService jiraService)
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

                var jiraIssues = await _jiraService.GetIssueAsync();

                if (JiraIssues.Count != 0)
                    JiraIssues.Clear();

                foreach (var issue in jiraIssues.Issues)
                {
                    JiraIssues.Add(issue);
                }
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
}