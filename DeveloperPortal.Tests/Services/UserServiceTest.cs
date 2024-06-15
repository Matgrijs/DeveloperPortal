using System;
using System.Threading.Tasks;
using DeveloperPortal.Services;
using DeveloperPortal.Services.Interfaces;
using Moq;
using Xunit;

namespace DeveloperPortal.Tests.Services
{
    [Collection("UserServiceTests")]
    public class UserServiceTest
    {
        private readonly Mock<IAuth0ManagementService> _auth0ManagementServiceMock;
        private readonly UserService _userService;
        private const string Token = "TestToken";

        public UserServiceTest()
        {
            _auth0ManagementServiceMock = new Mock<IAuth0ManagementService>();
            _userService = new UserService(_auth0ManagementServiceMock.Object);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnEmptyList_WhenTokenIsNullOrEmpty()
        {
            // Arrange
            _auth0ManagementServiceMock
                .Setup(x => x.GetManagementApiToken())
                .ReturnsAsync(string.Empty);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.GetUsersAsync());
        }

        [Fact]
        public async Task GetUsersAsync_ShouldThrowException_WhenGetManagementApiTokenThrowsException()
        {
            // Arrange
            _auth0ManagementServiceMock
                .Setup(x => x.GetManagementApiToken())
                .ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.GetUsersAsync());
        }
    }
}