using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using DeveloperPortal.Models.Chats;
using DeveloperPortal.Services;
using Newtonsoft.Json;

namespace DeveloperPortal;

public partial class DevChat
{
    private ObservableCollection<ChatMessage> Messages { get; }
    private HubConnection? _hubConnection;
    private readonly NavigationService _navigationService; 
    private static readonly HttpClient HttpClient = new();
    private readonly ApiService _apiService; 

    public DevChat(NavigationService navigationService, ApiService apiService)
    {
        InitializeComponent();
        
        _navigationService = navigationService;
        _apiService = apiService;

        Messages = new ObservableCollection<ChatMessage>();
        MessagesList.ItemsSource = Messages;

        Messages.CollectionChanged += (_, _) => ScrollToLastMessage();

        InitializeSignalR();
    }

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

            _hubConnection.On<ChatMessage>("ReceiveMessage", (chatMessage) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Messages.Add(new ChatMessage() 
                    { 
                        Id = chatMessage.Id,
                        Username = chatMessage.Username,
                        Auth0Id = chatMessage.Auth0Id,
                        Message = chatMessage.Message,
                        MessageTime = chatMessage.MessageTime,
                    });
                    
                    ScrollToLastMessage();
                });
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
                    ScrollToLastMessage(); // Scroll to the last message after loading messages
                });
            }
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }

    private async void OnSendMessage(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(MessageEntry.Text)) return;

        var message = new CreateChatDto()
        {
            Username = AuthenticationService.Instance.UserName,
            Auth0Id = AuthenticationService.Instance.Auth0Id,
            Message = MessageEntry.Text,
            MessageTime = DateTime.Now
        };

        var json = JsonConvert.SerializeObject(message);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await HttpClient.PostAsync($"{_apiService.BaseUrl}/api/Chat", content);
            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                MessageEntry.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }

    protected override async void OnDisappearing()
    {
        if (_hubConnection is { State: HubConnectionState.Connected })
        {
            await _hubConnection.StopAsync();
        }
        base.OnDisappearing();
    }

    private void OnIconClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            MessageEntry.Text += button.Text;
        }
    }

    private void ScrollToLastMessage()
    {
        if (Messages.Count > 0)
        {
            MessagesList.ScrollTo(Messages[^1], position: ScrollToPosition.End, animate: true);
        }
    }

    private void OnPageAppearing(object sender, EventArgs e)
    {
        ScrollToLastMessage();
    }
    
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await _navigationService.OnBackButtonClickedAsync();
    }
}
