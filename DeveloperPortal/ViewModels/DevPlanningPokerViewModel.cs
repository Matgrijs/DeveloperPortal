using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeveloperPortal.Services;
using DeveloperPortal.Models.Users;
using Microsoft.AspNetCore.SignalR.Client;

namespace DeveloperPortal.ViewModels
{
    public partial class DevPlanningPokerViewModel : BaseViewModel
    {
        private readonly UserService _userService;
        private HubConnection? _hubConnection;
        private readonly BaseHttpClientService _baseHttpClientService;

        public ObservableCollection<User> Users { get; } = new();

        public DevPlanningPokerViewModel(UserService userService, BaseHttpClientService baseHttpClientService)
        {
            _userService = userService;
            _baseHttpClientService = baseHttpClientService;
            InitializeSignalR();
            ButtonClickedCommand = new AsyncRelayCommand<string>(OnButtonClickedAsync);
        }

        public IAsyncRelayCommand<string> ButtonClickedCommand { get; }

        [ObservableProperty]
        private string? _selectedValue;

        private async void InitializeSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{_baseHttpClientService.Client.BaseAddress}/planningPokerHub")
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
                RefreshUsers();

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
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private async Task<PokerVote> GetExistingVote(string username)
        {
            var response = await _baseHttpClientService.Client.GetAsync($"{_baseHttpClientService.Client.BaseAddress}/api/Poker");
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

            var response = await _baseHttpClientService.Client.PostAsync($"{_baseHttpClientService.Client.BaseAddress}/api/Poker", content);
            response.EnsureSuccessStatusCode();
        }

        private async Task UpdateVoteAsync(PokerVote pokerVote)
        {
            var json = JsonSerializer.Serialize(pokerVote);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _baseHttpClientService.Client.PutAsync($"{_baseHttpClientService.Client.BaseAddress}/api/Poker/{pokerVote.Id}", content);
            response.EnsureSuccessStatusCode();
        }

        private void RefreshUsers()
        {
            var users = Users.ToList();
            Users.Clear();
            foreach (var user in users)
            {
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
