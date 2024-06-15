using System.Threading.Tasks;
using DeveloperPortal.Services.Interfaces;
using DeveloperPortal.ViewModels;
using Moq;
using Xunit;

namespace DeveloperPortal.Tests.ViewModels
{
    public class DashboardViewModelTest
    {
        private readonly Mock<INavigationService> _navigationServiceMock;
        private readonly DashboardViewModel _viewModel;

        public DashboardViewModelTest()
        {
            _navigationServiceMock = new Mock<INavigationService>();
            _viewModel = new DashboardViewModel(_navigationServiceMock.Object);
        }

        [Fact]
        public void DashboardViewModel_Initialize_CheckPropertyDefaults()
        {
            Assert.NotNull(_viewModel.NavigateCommand);
        }

        [Fact]
        public async Task NavigateCommand_Execute_ValidNavigation()
        {
            var destination = "TestDestination";

            // Setup the Mock
            _navigationServiceMock.Setup(n => n.NavigateToAsync(destination)).Returns(Task.CompletedTask);

            _viewModel.OnNavigate(destination);

            _navigationServiceMock.Verify(ns => ns.NavigateToAsync(destination), Times.Once);
        }

        [Fact]
        public void UpdateLabels_Invoked_ChangedPropertyValues()
        {
            _viewModel.UpdateLabels();

            Assert.NotNull(_viewModel.ChatLabel);
            Assert.NotNull(_viewModel.NotesLabel);
            Assert.NotNull(_viewModel.ProfileLabel);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task OnNavigate_NullOrEmpty_NotNavigate(string destination)
        {
            _viewModel.OnNavigate(destination);

            _navigationServiceMock.Verify(ns => ns.NavigateToAsync(It.IsAny<string>()), Times.Never);
        }
    }
}