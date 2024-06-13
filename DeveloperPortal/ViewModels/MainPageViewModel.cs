using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using DeveloperPortal.Services;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DeveloperPortal.Authentication;
using DeveloperPortal.Services.Interfaces;
using IdentityModel.Client;
using IdentityModel.OidcClient;

namespace DeveloperPortal.ViewModels
{
    public class MainPageViewModel : ObservableRecipient
    {
        private readonly OidcClient _authClient;
        private readonly INavigationService _navigationService;

        // Constructor will now take in an INavigationService instance
        public MainPageViewModel(OidcClient client, INavigationService navigationService)
        {
            this._authClient = client;
            this._navigationService = navigationService;
        }

        // Used for authentication
        public ICommand LoginCommand => new AsyncRelayCommand(OnLoginClickedAsync);

        // LoginCommand executes this function when a login attempt is made
        private async Task OnLoginClickedAsync()
        {
            Debug.WriteLine("aan het inloggen");
            try
            {
                // Attempt to log in and fetch user data and tokens
                var loginResult = await _authClient.LoginAsync(
                    new LoginRequest()
                    {
                        FrontChannelExtraParameters = new Parameters()
                        {
                            {"audience", AuthenticationConstants.Auth0Audience}
                        }
                    });

                ProcessRedirection();

                // If login was successful
                if (!loginResult.IsError)
                {
                    // Retrieve user, access token and user parameters
                    var user = loginResult.User;
                    var accessToken = loginResult.AccessToken;

                    var userName = user.FindFirst(c => c.Type == "name")?.Value ?? "Username";
                    var userId = user.FindFirst(c => c.Type == "user_id")?.Value ??
                                 user.FindFirst(c => c.Type == "sub")?.Value ?? "anonymous";

                    // Initialize the AuthenticationService
                    AuthenticationService.Instance.Initialize(userName, userId, accessToken);

                    // After successfully logging in, navigate to the Dashboard page
                    await _navigationService.NavigateToAsync("Dashboard");
                }
            }
            catch (Exception ex)
            {
                // If something goes wrong during login, capture and log the exception
                SentrySdk.CaptureException(ex);
            }
        }

        // Windows workaround to focus on the application
        private void ProcessRedirection()
        {
#if WINDOWS
            WinUI.App.ActivateApplication();
#endif
        }
    }
}