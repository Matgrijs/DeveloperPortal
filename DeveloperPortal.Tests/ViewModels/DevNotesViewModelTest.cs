using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPortal.Models.Notes;
using DeveloperPortal.Services;
using DeveloperPortal.Services.Helpers;
using DeveloperPortal.Services.Interfaces;
using DeveloperPortal.ViewModels;
using JetBrains.Annotations;
using Moq;
using Moq.Contrib.HttpClient;
using Xunit;

namespace DeveloperPortal.Tests.ViewModels
{
    [TestSubject(typeof(DevNotesViewModel))]
    public class DevNotesViewModelTest
    {
        private Mock<IHttpHandler> _mockHttpHandler;
        private DevNotesViewModel _systemUnderTest;

        public DevNotesViewModelTest()
        {
            _mockHttpHandler = new Mock<IHttpHandler>();
            _systemUnderTest = new DevNotesViewModel(_mockHttpHandler.Object);
        }

        // Your tests follow...

        [Fact]
        public async Task LoadNotesAsync_WhenCalled_ShouldHandleNullResponseAsync()
        {
            // Arrange
            _mockHttpHandler.Setup(h => h.GetAsync(It.IsAny<string>())).ReturnsAsync((HttpResponseMessage)null);

            // Act
            await _systemUnderTest.LoadNotesAsync();

            // Assert
            Assert.Empty(_systemUnderTest.Notes);
        }
        [Fact]
        public async Task CreateOrUpdateNoteAsync_WhenSelectedNoteIsNull_ShouldCallOnCreateNoteAsync()
        {
            // Arrange
            _systemUnderTest.SelectedNote = null;

            // Act
            await _systemUnderTest.CreateOrUpdateNoteAsync();

            // Assert
            _mockHttpHandler.Verify(h => h.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()), Times.Never);
        }

        [Fact]
        public async Task CreateOrUpdateNoteAsync_WhenSelectedNoteIsNotNull_ShouldCallOnEditNoteAsync()
        {
            // Arrange
            _systemUnderTest.SelectedNote = new Note(Guid.NewGuid(), "username", "auth0Id", "content");
            _systemUnderTest.NoteContent = "New Content";  // Set NoteContent

            // Act
            await _systemUnderTest.CreateOrUpdateNoteAsync();

            // Assert
            _mockHttpHandler.Verify(h => h.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>()), Times.Never);
        }
        
        [Fact]
        public void TestGetters()
        {
            //Assert
            Assert.Equal("Save", _systemUnderTest.SaveLabel);
            Assert.Equal("Delete", _systemUnderTest.DeleteLabel);
            Assert.Equal("Add description here...", _systemUnderTest.PlaceHolderLabel);
            Assert.Equal("Notes", _systemUnderTest.TitleLabel);
        }
    }
}