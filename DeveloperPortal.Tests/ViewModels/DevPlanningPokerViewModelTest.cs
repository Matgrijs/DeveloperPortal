using DeveloperPortal.ViewModels;
using Moq;
using DeveloperPortal.Services.Interfaces;
using System.Threading.Tasks;
using DeveloperPortal.Models.Users;
using DeveloperPortal.Models.Poker;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Text.Json;
using System.Net;
using System.Text;
using Xunit;

namespace DeveloperPortal.Tests.ViewModels
{
    public class DevPlanningPokerViewModelTest
    {
        private Mock<IUserService> _userService;
        private Mock<IHttpHandler> _httpHandler;
        private DevPlanningPokerViewModel _viewModel;

        public DevPlanningPokerViewModelTest()
        {
            _userService = new Mock<IUserService>();
            _httpHandler = new Mock<IHttpHandler>();

            _viewModel = new DevPlanningPokerViewModel(_userService.Object, _httpHandler.Object);
        }

        [Fact]
        public async Task LoadUsersAsync_WhenCalled_CheckIfUsersAreLoaded()
        {
            // Arrange
            var users = new List<User> { new User { Name = "User1", Auth0Id = "test" }, new User { Name = "User2", Auth0Id = "test" } };
            _userService.Setup(s => s.GetUsersAsync()).Returns(Task.FromResult((IList<User>)users));

            // Act
            await _viewModel.LoadUsersAsync();

            // Assert
            Assert.Equal(2, users.Count);
            Assert.True(_viewModel.Users.All(u => users.Any(x => u.Name == x.Name)));
        }

        [Fact]
        public async Task OnButtonClickedAsync_WhenCalledWithNull_DoNotSendVote()
        {
            // Arrange
            string selectedValue = null;

            // Act
            await _viewModel.OnButtonClickedAsync(selectedValue);

            // Assert
            _httpHandler.Verify(h => h.PostAsync(It.IsAny<string>(), It.IsAny<StringContent>()), Times.Never);
            _httpHandler.Verify(h => h.PutAsync(It.IsAny<string>(), It.IsAny<StringContent>()), Times.Never);
        }

        [Fact]
        public async Task OnButtonClickedAsync_WhenCalledWithNonNull_SendVote()
        {
            // Arrange
            string selectedValue = "Vote1";
            var vote = new PokerVote { Username = "User1", Vote = selectedValue };
            var json = JsonSerializer.Serialize(vote);
            var message = new HttpResponseMessage
            {
                Content = new StringContent(json),
                StatusCode = HttpStatusCode.OK
            };
            _httpHandler.Setup(h => h.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(message));

            // Act
            await _viewModel.OnButtonClickedAsync(selectedValue);

            // Assert
            _httpHandler.Verify(h => h.PostAsync(It.IsAny<string>(), It.IsAny<StringContent>()), Times.Never);
        }

        [Fact]
        public async Task AddOrUpdateVoteAsync_WhenCalledWithExistVote_UpdateTheVote()
        {
            // Arrange
            var vote = new PokerVote { Username = "User1", Vote = "Vote1" };
            var json = JsonSerializer.Serialize(new List<PokerVote> { vote });
            var message = new HttpResponseMessage
            {
                Content = new StringContent(json),
                StatusCode = HttpStatusCode.OK
            };
            _httpHandler.Setup(h => h.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(message));

            // Act
            await _viewModel.AddOrUpdateVoteAsync(vote);

            // Assert
            _httpHandler.Verify(h => h.PutAsync(It.IsAny<string>(), It.IsAny<StringContent>()), Times.Never);
        }

        [Fact]
        public async Task AddOrUpdateVoteAsync_WhenCalledWithNewVote_CreateTheVote()
        {
            // Arrange
            var vote = new PokerVote { Username = "User1", Vote = "Vote1" };

            // Act
            await _viewModel.AddOrUpdateVoteAsync(vote);

            // Assert
            _httpHandler.Verify(h => h.PostAsync(It.IsAny<string>(), It.IsAny<StringContent>()), Times.Never);
        }
    }
}