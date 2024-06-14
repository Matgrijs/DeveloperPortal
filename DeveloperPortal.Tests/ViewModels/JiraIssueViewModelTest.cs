using System.Collections.Generic;
using DeveloperPortal.ViewModels;
using DeveloperPortal.Services.Interfaces;
using DeveloperPortal.Models.JiraIssues;
using Moq;
using Xunit;
using System.Threading.Tasks;

namespace DeveloperPortal.Tests.ViewModels
{
    [Collection("Sequential")]
    public class JiraIssueViewModelTest
    {
        private readonly JiraIssueViewModel _viewModel;
        private readonly Mock<IJiraService> _mockJiraService;

        public JiraIssueViewModelTest()
        {
            _mockJiraService = new Mock<IJiraService>();
            _viewModel = new JiraIssueViewModel(_mockJiraService.Object);
        }

        [Fact]
        public async Task GetJiraIssues_IsBusy_False_DoesNotFetchIssues()
        {
            _viewModel.IsBusy = true;

            await _viewModel.GetJiraIssues();
            _mockJiraService.Verify(service => service.GetIssueAsync(), Times.Never);
        }

        [Fact]
        public async Task GetJiraIssues_IsBusy_False_FetchesAndAddsIssues()
        {
            _viewModel.IsBusy = false;
            _mockJiraService.Setup(service => service.GetIssueAsync())
                .ReturnsAsync(new JiraSearchResults
                {
                    Issues = new List<JiraIssue> { new JiraIssue("1", "Test", new Fields
                    {
                        Summary = "title",
                        Description = "culprit",
                        IssueType = new IssueType { Name = "Bug" },
                        Project = new Project { Key = "DVP" }
                    }) }
                });

            await _viewModel.GetJiraIssues();

            Assert.NotEmpty(_viewModel.JiraIssues);
            _mockJiraService.Verify(service => service.GetIssueAsync(), Times.Once);
        }

        [Fact]
        public async Task GetJiraIssues_ServiceReturnsNull_DoesNotAddIssues()
        {
            _viewModel.IsBusy = false;
            _mockJiraService.Setup(service => service.GetIssueAsync())
                .ReturnsAsync((JiraSearchResults)null);

            await _viewModel.GetJiraIssues();

            Assert.Empty(_viewModel.JiraIssues);
        }

        [Fact]
        public async Task GetJiraIssues_IssuesCollectionNotEmpty_ClearsCollection()
        {
            _viewModel.IsBusy = false;
            _viewModel.JiraIssues.Add(new JiraIssue("1", "Test",new Fields
            {
                Summary = "title",
                Description = "culprit",
                IssueType = new IssueType { Name = "Bug" },
                Project = new Project { Key = "DVP" }
            }));
            _mockJiraService.Setup(service => service.GetIssueAsync())
                .ReturnsAsync(new JiraSearchResults());

            await _viewModel.GetJiraIssues();

            Assert.Empty(_viewModel.JiraIssues);
        }
    }
}