using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeveloperPortal.Models.Chats;
using DeveloperPortal.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Plugin.Maui.Audio;

namespace DeveloperPortal.ViewModels
{
    public partial class DevChatViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private HubConnection? _hubConnection;
        private static readonly HttpClient HttpClient = new();
        private readonly IAudioManager _audioManager;

        public ObservableCollection<ChatMessage> Messages { get; } = new();

        public DevChatViewModel(ApiService apiService)
        {
            Title = "Dev Chat";
            _apiService = apiService;
            _audioManager = AudioManager.Current;
            InitializeSignalR();
            SendMessageCommand = new AsyncRelayCommand(OnSendMessageAsync);
            IconClickedCommand = new RelayCommand<string>(OnIconClicked);
            ShowIconsCommand = new RelayCommand(ToggleEmojiList);
        }

        public IAsyncRelayCommand SendMessageCommand { get; }
        public IRelayCommand<string> IconClickedCommand { get; }
        public IRelayCommand ShowIconsCommand { get; }

        public event Action ScrollToLastMessageRequested;
        public event Action<bool> AnimateEmojiListRequested;

        private async void InitializeSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{_apiService.BaseUrl}/chatHub")
                .WithAutomaticReconnect()
                .Build();

            try
            {
                await _hubConnection.StartAsync();
                await LoadMessagesAsync();

                _hubConnection.On<ChatMessage>("ReceiveMessage", async (chatMessage) =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Messages.Add(new ChatMessage
                        {
                            Id = chatMessage.Id,
                            Username = chatMessage.Username,
                            Auth0Id = chatMessage.Auth0Id,
                            Message = chatMessage.Message,
                            MessageTime = chatMessage.MessageTime,
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
                string url = $"{_apiService.BaseUrl}/api/Chat";
                var response = await HttpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var messages = JsonConvert.DeserializeObject<List<ChatMessage>>(json);
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        if (messages != null)
                        {
                            foreach (var message in messages)
                            {
                                Messages.Add(message);
                            }
                        }
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
                var response = await HttpClient.PostAsync($"{_apiService.BaseUrl}/api/Chat", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageEntryText = string.Empty;
                }
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

        [ObservableProperty]
        private string messageEntryText;

        [ObservableProperty]
        private bool isEmojiListVisible;

        private void ScrollToLastMessage()
        {
            ScrollToLastMessageRequested?.Invoke();
        }

        public async void OnDisappearing()
        {
            if (_hubConnection is { State: HubConnectionState.Connected })
            {
                await _hubConnection.StopAsync();
            }
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
}
