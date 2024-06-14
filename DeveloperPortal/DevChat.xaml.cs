using DeveloperPortal.Services.Interfaces;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class DevChat
{
    private readonly DevChatViewModel _viewModel;

    public DevChat(IHttpHandler httpsHelper)
    {
        InitializeComponent();

        _viewModel = new DevChatViewModel(httpsHelper);
        BindingContext = _viewModel;
        _viewModel.ScrollToLastMessageRequested += ScrollToLastMessage;
        _viewModel.AnimateEmojiListRequested += AnimateEmojiList;
    }

    private void OnPageAppearing(object sender, EventArgs e)
    {
        ScrollToLastMessage();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.OnDisappearing();
        _viewModel.ScrollToLastMessageRequested -= ScrollToLastMessage;
        _viewModel.AnimateEmojiListRequested -= AnimateEmojiList;
    }

    private void ScrollToLastMessage()
    {
        if (_viewModel.Messages.Count > 0)
            MessagesList.ScrollTo(_viewModel.Messages[^1], position: ScrollToPosition.End, animate: true);
    }

    private async void AnimateEmojiList(bool show)
    {
        if (show)
        {
            EmojiList.IsVisible = true;
            await EmojiList.FadeTo(1, 500);
        }
        else
        {
            await EmojiList.FadeTo(0, 500);
            EmojiList.IsVisible = false;
        }
    }
}