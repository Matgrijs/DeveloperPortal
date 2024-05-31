using Auth0.OidcClient;
using DeveloperPortal.Services;
using System.Diagnostics;
using System.Linq;

namespace DeveloperPortal
{
    public partial class MainPage
    {
        private readonly Auth0Client _auth0Client;

        public MainPage(Auth0Client client)
        {
            InitializeComponent();
            _auth0Client = client;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                var loginResult = await _auth0Client.LoginAsync();

                if (!loginResult.IsError)
                {
                    var user = loginResult.User;
                    var accessToken = loginResult.AccessToken;

                    // Debug information
                    foreach (var claim in user.Claims)
                    {
                        Debug.WriteLine($"Claim: {claim.Type} - {claim.Value}");
                    }

                    var userName = user.FindFirst(c => c.Type == "name")?.Value ?? "Username";
                    var userId = user.FindFirst(c => c.Type == "user_id")?.Value ?? user.FindFirst(c => c.Type == "sub")?.Value ?? "anonymous";
                    
                    AuthenticationService.Instance.Initialize(userName, userId, accessToken);
                    
                    await Navigation.PushAsync(new Dashboard());
                }
                else
                {
                    await DisplayAlert("Error", loginResult.ErrorDescription, "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                SentrySdk.CaptureException(ex);
            }
        }
    }
}