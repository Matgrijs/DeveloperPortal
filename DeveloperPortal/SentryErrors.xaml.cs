using System.Collections.ObjectModel;
using System.Diagnostics;
using DeveloperPortal.Services;

namespace DeveloperPortal
{
    public partial class SentryErrors
    {
        private readonly SentryService _sentryService;
        private readonly JiraService _jiraService;
        private readonly NavigationService _navigationService;
        private ObservableCollection<SentryError> Errors { get; set; }

        public SentryErrors(SentryService sentryService, JiraService jiraService, NavigationService navigationService)
        {
            InitializeComponent();
            _sentryService = sentryService;
            _jiraService = jiraService;
            _navigationService = navigationService;
            Errors = new ObservableCollection<SentryError>();
            ErrorsListView.ItemsSource = Errors;
            LoadErrors();
        }

        private async void LoadErrors()
        {
            try
            {
                var errors = await _sentryService.GetSentryErrorsAsync();
                foreach (var error in errors)
                {
                    var sentryError = new SentryError
                    {
                        Id = (int)error["id"],
                        Title = error["title"].ToString(),
                        Culprit = error["culprit"]?.ToString()
                    };
                    Errors.Add(sentryError);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void CreateJiraBug(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var error = button.BindingContext as SentryError;
                if (error != null)
                {
                    var title = error.Title;
                    var description = error.Culprit;
                    if (!string.IsNullOrEmpty(title))
                    {
                        var jiraIssueCreateRequest = new JiraIssueCreateRequest
                        {
                            Fields = new Fields
                            {
                                Summary = title,
                                Description = description,
                                IssueType = new IssueType { Name = "Bug" },
                                Project = new Project { Key = "DVP" }
                            }
                        };

                        try
                        {
                            await _jiraService.CreateIssueAsync(jiraIssueCreateRequest);
                        }
                        catch (Exception ex)
                        {
                            SentrySdk.CaptureException(ex);
                        }
                    }
                }
            }
        }
    
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            _navigationService.OnBackButtonClickedAsync();
        }
    }

    public class SentryError
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Culprit { get; set; }
    }
}