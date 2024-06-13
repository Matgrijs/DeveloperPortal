using DeveloperPortal.Services;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class JiraIssues : ContentPage
{
    public JiraIssues(JiraService jiraService)
    {
        InitializeComponent();
        BindingContext = new JiraIssueViewModel(jiraService);

        // Call the GetJiraIssuesAsync method when the page appears
        Appearing += async (sender, e) => await ((JiraIssueViewModel)BindingContext).GetJiraIssues();
    }
}