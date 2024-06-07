using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DeveloperPortal.Models.SentryErrors;
using DeveloperPortal.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeveloperPortal.Models.JiraIssues;
using Sentry;
using System.Diagnostics;

namespace DeveloperPortal.ViewModels
{
    public partial class SentryErrorViewModel : BaseViewModel
    {
        private readonly SentryService _sentryService;
        private readonly JiraService _jiraService;

        public ObservableCollection<SentryError> Errors { get; } = new();

        public SentryErrorViewModel(SentryService sentryService, JiraService jiraService)
        
        {
            _sentryService = sentryService;
            _jiraService = jiraService;
            Title = "Sentry Errors";
            CreateJiraBugCommand = new AsyncRelayCommand<SentryError>(CreateJiraBugAsync);
        }

        public IAsyncRelayCommand<SentryError> CreateJiraBugCommand { get; }

        public async Task LoadErrors()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var errors = await _sentryService.GetSentryErrors();

                if (Errors.Count != 0)
                    Errors.Clear();

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
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task CreateJiraBugAsync(SentryError error)
        {
            if (error == null || string.IsNullOrEmpty(error.Title))
                return;

            var jiraIssueCreateRequest = new CreateJiraIssueDto
            {
                Fields = new Fields
                {
                    Summary = error.Title,
                    Description = error.Culprit,
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
