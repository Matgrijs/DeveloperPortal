using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeveloperPortal.Models.Poker;
using DeveloperPortal.Models.Users;
using DeveloperPortal.Services;
using DeveloperPortal.Services.DevHttpsConnectionHelper;
using Microsoft.AspNetCore.SignalR.Client;

namespace DeveloperPortal.ViewModels;

public partial class DevPlanningPokerViewModel : BaseViewModel
{
    private readonly IDevHttpsConnectionHelper _httpsHelper;
    private readonly UserService _userService;
    private HubConnection? _hubConnection;

    [ObservableProperty] private string? _selectedValue;

    public DevPlanningPokerViewModel(UserService userService, IDevHttpsConnectionHelper httpsHelper)
    {
        _userService = userService;
        _httpsHelper = httpsHelper;
        InitializeSignalR();
        ButtonClickedCommand = new AsyncRelayCommand<string>(OnButtonClickedAsync);
    }


    public ObservableCollection<User> Users { get; } = new();

    public IAsyncRelayCommand<string> ButtonClickedCommand { get; }

    private async void InitializeSignalR()
    {
        _hubConnection = new HubConnectionBuilder()
#if ANDROID
                .WithUrl(_httpsHelper.DevServerRootUrl + "/planningPokerHub"
                    , configureHttpConnection: o =>
                    {
                        o.HttpMessageHandlerFactory = m => _httpsHelper.GetPlatformMessageHandler();
                    }
                )
#else
            .WithUrl(_httpsHelper.DevServerRootUrl + "/planningPokerHub")
#endif
            .WithAutomaticReconnect()
            .Build();

        try
        {
            await _hubConnection.StartAsync();

            _hubConnection.On<PokerVote>("ReceiveVote", _ =>
            {
                Debug.WriteLine("triggered");

                async void Action()
                {
                    await LoadUsersAsync();
                }

                MainThread.BeginInvokeOnMainThread(Action);
            });
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }

    public async Task LoadUsersAsync()
    {
        try
        {
            var users = await _userService.GetUsersAsync();
            Users.Clear();
            foreach (var user in users)
            {
                var vote = await GetExistingVote(user.Auth0Id);
                if (vote != null)
                {
                    user.Name = vote.Username;
                    user.Vote = vote.Vote;
                    Debug.WriteLine(user.Vote);
                }

                Users.Add(user);
            }
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }

    public async Task OnButtonClickedAsync(string? selectedValue)
    {
        if (selectedValue != null)
        {
            var pokerVote = new PokerVote
            {
                Username = AuthenticationService.Instance.UserName,
                auth0Id = AuthenticationService.Instance.Auth0Id,
                Vote = selectedValue
            };

            await AddOrUpdateVoteAsync(pokerVote);
        }
    }

    private async Task AddOrUpdateVoteAsync(PokerVote pokerVote)
    {
        try
        {
            var existingVote = await GetExistingVote(pokerVote.auth0Id);

            if (existingVote != null)
            {
                pokerVote.Id = existingVote.Id;
                await UpdateVoteAsync(pokerVote);
            }
            else
            {
                await CreateVoteAsync(pokerVote);
            }
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }

    public async Task<PokerVote?> GetExistingVote(string auth0Id)
    {
        var httpClient = _httpsHelper.HttpClient;
        var response = await httpClient.GetAsync($"{_httpsHelper.DevServerRootUrl}/api/Poker");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            if (json.Length > 0)
            {
                var pokerVotes = JsonSerializer.Deserialize<List<PokerVote>>(json);
                return pokerVotes?.FirstOrDefault(v => v.auth0Id == auth0Id);
            }
        }

        return null;
    }

    private async Task CreateVoteAsync(PokerVote pokerVote)
    {
        var json = JsonSerializer.Serialize(pokerVote);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var httpClient = _httpsHelper.HttpClient;
        var response = await httpClient.PostAsync($"{_httpsHelper.DevServerRootUrl}/api/Poker", content);
        response.EnsureSuccessStatusCode();
    }

    private async Task UpdateVoteAsync(PokerVote pokerVote)
    {
        var json = JsonSerializer.Serialize(pokerVote);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var httpClient = _httpsHelper.HttpClient;
        var response = await httpClient.PutAsync($"{_httpsHelper.DevServerRootUrl}/api/Poker/{pokerVote.Id}", content);
        response.EnsureSuccessStatusCode();
    }
}