using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeveloperPortal.Services;
using DeveloperPortal.Models.Users;
using DeveloperPortal.Services.DevHttpsConnectionHelper;
using Microsoft.AspNetCore.SignalR.Client;
using Debug = System.Diagnostics.Debug;

namespace DeveloperPortal.ViewModels
{
    public partial class DevPlanningPokerViewModel : BaseViewModel
    {
        private readonly UserService _userService;
        private HubConnection? _hubConnection;
        private readonly IDevHttpsConnectionHelper _httpsHelper;

        public ObservableCollection<User> Users { get; } = new();

        public DevPlanningPokerViewModel(UserService userService, IDevHttpsConnectionHelper httpsHelper)
        {
            _userService = userService;
            _httpsHelper = httpsHelper;
            InitializeSignalR();
            ButtonClickedCommand = new AsyncRelayCommand<string>(OnButtonClickedAsync);
        }

        public IAsyncRelayCommand<string> ButtonClickedCommand { get; }

        [ObservableProperty]
        private string? _selectedValue;

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

                _hubConnection.On<PokerVote>("ReceiveVote", (vote) =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        var user = Users.FirstOrDefault(u => u.Name == vote.Username);
                        if (user != null)
                        {
                            user.SelectedValue = vote.Vote;
                            RefreshUsers();
                        }
                    });
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
                    Users.Add(user);
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private async Task OnButtonClickedAsync(string? selectedValue)
        {
            var loggedInUser = Users.FirstOrDefault(u => u.Name == AuthenticationService.Instance.UserName);

            if (loggedInUser != null)
            {
                loggedInUser.SelectedValue = selectedValue;

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
                var existingVote = await GetExistingVote(pokerVote.Username);
                
                if (existingVote != null)
                {
                    pokerVote.Id = existingVote.Id;
                    await UpdateVoteAsync(pokerVote);
                }
                else
                {
                    await CreateVoteAsync(pokerVote);
                }

                RefreshUsers();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private async Task<PokerVote> GetExistingVote(string username)
        {
            var httpClient = _httpsHelper.HttpClient;
            var response = await httpClient.GetAsync($"{_httpsHelper.DevServerRootUrl}/api/Poker");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var pokerVotes = JsonSerializer.Deserialize<List<PokerVote>>(json);
                return pokerVotes?.FirstOrDefault(v => v.Username == username);
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

        private void RefreshUsers()
        {
            var users = Users.ToList();
            Users.Clear();
            foreach (var user in users)
            {
                Debug.WriteLine(user.SelectedValue);
                Users.Add(user);
            }
        }
    }

    public class PokerVote
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string auth0Id { get; set; }
        public string Vote { get; set; }
    }
}
