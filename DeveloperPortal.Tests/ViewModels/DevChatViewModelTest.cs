using Xunit;
using Moq;
using DeveloperPortal.Services.Interfaces;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal.Tests.ViewModels
{
    public class DevChatViewModelTest
    {
        private readonly Mock<IHttpHandler> _mockHttpHandler = new Mock<IHttpHandler>();
        private readonly DevChatViewModel _viewModel;

        public DevChatViewModelTest()
        {
            _viewModel = new DevChatViewModel(_mockHttpHandler.Object);
        }

        [Fact]
        public void InitialState_Test()
        {
            Assert.False(_viewModel.IsEmojiListVisible);
            Assert.Equal(null, _viewModel.MessageEntryText);
            Assert.NotNull(_viewModel.TitleLabel);
            Assert.NotNull(_viewModel.PlaceHolderLabel);
            Assert.NotNull(_viewModel.SendLabel);
            Assert.Empty(_viewModel.Messages);
        }

        [Fact]
        public void IconClickedCommand_Executed_Test()
        {
            var prevMessageEntryText = _viewModel.MessageEntryText;
            var icon = "icon_dummy";

            // Executing the command
            _viewModel.IconClickedCommand.Execute(icon);

            // Checking if icon is added to MessageEntryText
            Assert.Equal(prevMessageEntryText + icon, _viewModel.MessageEntryText);
        }

        [Fact]
        public void ShowIconsCommand_Transitions_Test()
        {
            Assert.False(_viewModel.IsEmojiListVisible);

            // Toggling
            _viewModel.ShowIconsCommand.Execute(null);

            Assert.True(_viewModel.IsEmojiListVisible);

            // Toggling again
            _viewModel.ShowIconsCommand.Execute(null);

            Assert.False(_viewModel.IsEmojiListVisible);
        }

        [Fact]
        public void SendMessage_EmptyMessage_Test()
        {
            _viewModel.MessageEntryText = "";

            // "Sending" empty message
            _viewModel.SendMessageCommand.Execute(null);

            // Since the message is empty, nothing should have happened
            _mockHttpHandler.Verify(m => m.HttpClient, Times.Never());
        }

        [Fact]
        public void SendMessage_SampleMessage_Test()
        {
            _viewModel.MessageEntryText = "Hello World!";

            // "Sending" a valid message
            _viewModel.SendMessageCommand.Execute(null);

            // Make sure it makes a request to the server
            _mockHttpHandler.Verify(m => m.HttpClient, Times.Once());
        }
    }
}