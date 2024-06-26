using System.Collections.ObjectModel;
using System.Globalization;
using System.Resources;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeveloperPortal.Models.Chats;
using DeveloperPortal.Resources;
using DeveloperPortal.Services;
using DeveloperPortal.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Plugin.Maui.Audio;

namespace DeveloperPortal.ViewModels;

public partial class DevChatViewModel : BaseViewModel
{
    private readonly IAudioManager _audioManager;
    private readonly IHttpHandler _httpsHelper;
    private readonly ResourceManager _resourceManager = new(typeof(AppResources));
    private HubConnection? _hubConnection;

    [ObservableProperty] private bool _isEmojiListVisible;

    [ObservableProperty] private string _messageEntryText = null!;

    public DevChatViewModel(IHttpHandler httpsHelper)
    {
        _audioManager = AudioManager.Current;
        _httpsHelper = httpsHelper;
        InitializeSignalR();
        SendMessageCommand = new AsyncRelayCommand(OnSendMessageAsync);
        IconClickedCommand = new RelayCommand<string>(OnIconClicked!);
        ShowIconsCommand = new RelayCommand(ToggleEmojiList);
    }

    public string? TitleLabel => _resourceManager.GetString("ChatTitle", CultureInfo.CurrentCulture);
    public string? PlaceHolderLabel => _resourceManager.GetString("AddMessage", CultureInfo.CurrentCulture);
    public string? SendLabel => _resourceManager.GetString("Send", CultureInfo.CurrentCulture);

    public ObservableCollection<ChatMessage> Messages { get; } = new();

    public IAsyncRelayCommand SendMessageCommand { get; }
    public IRelayCommand<string> IconClickedCommand { get; }
    public IRelayCommand ShowIconsCommand { get; }

    public event Action ScrollToLastMessageRequested = null!;
    public event Action<bool> AnimateEmojiListRequested = null!;

    private async void InitializeSignalR()
    {
        _hubConnection = new HubConnectionBuilder()
#if ANDROID
                .WithUrl(_httpsHelper.DevServerRootUrl + "/chatHub"
                    , configureHttpConnection: o =>
                    {
                        o.HttpMessageHandlerFactory = m => _httpsHelper.GetPlatformMessageHandler();
                    }
                )
#else
            .WithUrl(_httpsHelper.DevServerRootUrl + "/chatHub")
#endif
            .WithAutomaticReconnect()
            .Build();

        try
        {
            await _hubConnection.StartAsync();
            await LoadMessagesAsync();

            _hubConnection.On<ChatMessage>("ReceiveMessage", async chatMessage =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Messages.Add(new ChatMessage
                    {
                        Id = chatMessage.Id,
                        Username = chatMessage.Username,
                        Auth0Id = chatMessage.Auth0Id,
                        Message = chatMessage.Message,
                        MessageTime = chatMessage.MessageTime
                    });
                    ScrollToLastMessage();
                });
                await PlayNotificationSoundAsync();
            });
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }

    private async Task LoadMessagesAsync()
    {
        try
        {
            var httpClient = _httpsHelper.HttpClient;
            var url = $"{_httpsHelper.DevServerRootUrl}/api/Chat";
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var messages = JsonConvert.DeserializeObject<List<ChatMessage>>(json);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (messages != null)
                        foreach (var message in messages)
                            Messages.Add(message);
                    ScrollToLastMessage();
                });
            }
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }

    private async Task OnSendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(MessageEntryText)) return;

        var message = new CreateChatDto
        {
            Username = AuthenticationService.Instance.UserName,
            Auth0Id = AuthenticationService.Instance.Auth0Id,
            Message = MessageEntryText,
            MessageTime = DateTime.Now
        };

        var json = JsonConvert.SerializeObject(message);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var httpClient = _httpsHelper.HttpClient;
            var response = await httpClient.PostAsync($"{_httpsHelper.DevServerRootUrl}/api/Chat", content);
            if (response.IsSuccessStatusCode) MessageEntryText = string.Empty;
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }

    private void OnIconClicked(string icon)
    {
        MessageEntryText += icon;
    }

    private void ToggleEmojiList()
    {
        AnimateEmojiListRequested?.Invoke(!IsEmojiListVisible);
        IsEmojiListVisible = !IsEmojiListVisible;
    }

    private void ScrollToLastMessage()
    {
        ScrollToLastMessageRequested?.Invoke();
    }

    public async void OnDisappearing()
    {
        if (_hubConnection is { State: HubConnectionState.Connected }) await _hubConnection.StopAsync();
    }

    private async Task PlayNotificationSoundAsync()
    {
        try
        {
            var fileStream = await FileSystem.OpenAppPackageFileAsync("alert.wav");
            var player = _audioManager.CreatePlayer(fileStream);
            player.Play();
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }
}