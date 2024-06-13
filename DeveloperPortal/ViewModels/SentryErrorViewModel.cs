using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using DeveloperPortal.Models.JiraIssues;
using DeveloperPortal.Models.SentryErrors;
using DeveloperPortal.Services;

namespace DeveloperPortal.ViewModels;

public class SentryErrorViewModel : BaseViewModel
{
    private readonly JiraService _jiraService;
    private readonly SentryService _sentryService;

    public SentryErrorViewModel(SentryService sentryService, JiraService jiraService)

    {
        _sentryService = sentryService;
        _jiraService = jiraService;
        Title = "Sentry Errors";
        CreateJiraBugCommand = new AsyncRelayCommand<SentryError>(CreateJiraBugAsync!);
    }

    public ObservableCollection<SentryError> Errors { get; } = new();

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
                    Id = (int)error["id"]!,
                    Title = error["title"]!.ToString(),
                    Culprit = error["culprit"]?.ToString()
                };
                Errors.Add(sentryError);
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

    private async Task CreateJiraBugAsync(SentryError error)
    {
        if (string.IsNullOrEmpty(error.Title))
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