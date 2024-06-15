using DeveloperPortal.ViewModels;
using JetBrains.Annotations;
using DeveloperPortal.Services.Interfaces;
using DeveloperPortal.Models.SentryErrors;
using Moq;
using Xunit;
using System.Threading.Tasks;
using DeveloperPortal.Models.JiraIssues;
using Newtonsoft.Json.Linq;

namespace DeveloperPortal.Tests.ViewModels;

[TestSubject(typeof(SentryErrorViewModel))]
public class SentryErrorViewModelTest
{
    private readonly Mock<ISentryService> _mockSentryService = new();
    private readonly Mock<IJiraService> _mockJiraService = new();

    private SentryErrorViewModel CreateViewModel() => new(_mockSentryService.Object, _mockJiraService.Object);

    [Fact]
    public void SentryErrorViewModelTest_Title_IsSetCorrectly()
    {
        var viewModel = CreateViewModel();
        Assert.Equal("Sentry Errors", viewModel.Title);
    }

    [Fact]
    public async Task LoadErrors_OnInvoke_UpdatesErrorsCollection()
    {
        var viewModel = CreateViewModel();
        _mockSentryService.Setup(s => s.GetSentryErrors()).ReturnsAsync(new JArray(
            new JObject { ["id"] = 1, ["title"] = "Error1", ["culprit"] = "Cause1" },
            new JObject { ["id"] = 2, ["title"] = "Error2", ["culprit"] = "Cause2" }
        ));
        await viewModel.LoadErrors();
        Assert.Equal(2, viewModel.Errors.Count);
    }

    [Fact]
    public async Task CreateJiraBugCommand_OnInvoke_WithNullTitle_DoesNotCreateIssue()
    {
        var viewModel = CreateViewModel();
        var sentryError = new SentryError { Title = null, Id = 123, Culprit = "Some cause" };

        await viewModel.CreateJiraBugCommand.ExecuteAsync(sentryError);

        _mockJiraService.Verify(s => s.CreateIssueAsync(It.IsAny<CreateJiraIssueDto>()), Times.Never);
    }

    [Fact]
    public async Task CreateJiraBugCommand_OnInvoke_WithTitle_CreatesIssueWithSameTitle()
    {
        var viewModel = CreateViewModel();
        var sentryError = new SentryError { Title = "Test error", Id = 123, Culprit = "Some cause" };

        await viewModel.CreateJiraBugCommand.ExecuteAsync(sentryError);

        _mockJiraService.Verify(
            s => s.CreateIssueAsync(It.Is<CreateJiraIssueDto>(issue => issue.Fields.Summary == "Test error")),
            Times.Once);
    }

    [Fact]
    public async Task LoadErrors_WhenIsBusy_DoesNotFetchData()
    {
        var viewModel = CreateViewModel();
        viewModel.IsBusy = true;

        await viewModel.LoadErrors();

        Assert.Empty(viewModel.Errors);
        _mockSentryService.Verify(s => s.GetSentryErrors(), Times.Never);
    }
}