using DeveloperPortal.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Collections.ObjectModel;
using DeveloperPortal.Services;

namespace DeveloperPortal;

public partial class DevPlanningPoker
{
    private readonly UserService _userService;
    private readonly string _loggedInUserName;
    private HubConnection? _hubConnection;
    private static readonly ApiService ApiService = new ApiService();
    private ObservableCollection<UserService.CustomUser> Users { get; set; }

    public DevPlanningPoker(UserService userService)
    {
        InitializeComponent();
        _userService = userService;
        _loggedInUserName = AuthenticationService.Instance.UserName;
        Users = new ObservableCollection<UserService.CustomUser>();

        UsersCollectionView.ItemsSource = Users;
        LoadUsers();
        InitializeSignalR();
    }

    private async void InitializeSignalR()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{ApiService.BaseUrl}/planningPokerHub")
            .WithAutomaticReconnect()
            .Build();

        try
        {
            await _hubConnection.StartAsync();

            _hubConnection.On<string, string>("ReceiveVote", (userName, vote) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var user = Users.FirstOrDefault(u => u.Name == userName);
                    if (user != null)
                    {
                        user.SelectedValue = vote;
                        UsersCollectionView.ItemsSource = null;
                        UsersCollectionView.ItemsSource = Users;
                    }
                });
            });
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }

    private async void LoadUsers()
    {
        try
        {
            var users = await _userService.GetUsersAsync();
            foreach (var user in users)
            {
                Users.Add(user);
            }
            UsersCollectionView.ItemsSource = Users;
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            var selectedValue = button.Text;
            var loggedInUser = Users.FirstOrDefault(u => u.Name == _loggedInUserName);

            if (loggedInUser != null)
            {
                loggedInUser.SelectedValue = selectedValue;
                UsersCollectionView.ItemsSource = null;
                UsersCollectionView.ItemsSource = Users;

                if (_hubConnection != null)
                {
                    await _hubConnection.InvokeAsync("SendVote", _loggedInUserName, selectedValue);
                }
            }
        }
    }

    private void OnBackButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}