using System.Collections.ObjectModel;
using DeveloperPortal.Services;

namespace DeveloperPortal;

public partial class JiraIssues
{
    public ObservableCollection<JiraIssue> Issues { get; set; }
    private readonly NavigationService _navigationService;
    public JiraIssues(NavigationService navigationService)
    {
        InitializeComponent();
        _navigationService = navigationService;
        
        Issues = new ObservableCollection<JiraIssue>();
        IssuesCollectionView.ItemsSource = Issues;
        LoadIssues();
    }

    private async void LoadIssues()
    {
        try
        {
            var jiraService = new JiraService();
            var issues = await jiraService.GetIssuesByTypeAsync("Bug");
            foreach (var issue in issues)
            {
                Issues.Add(issue);
            }
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }
    
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        _navigationService.OnBackButtonClickedAsync();
    }
}
