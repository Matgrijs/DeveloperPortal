using Auth0.OidcClient;
using DeveloperPortal.Services;

namespace DeveloperPortal
{
    public partial class MainPage
    {
        private readonly Auth0Client _auth0Client;

        public MainPage(Auth0Client auth0Client)
        {
            InitializeComponent();
            _auth0Client = auth0Client;
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
                SentrySdk.CaptureException(ex);
            }
        }
    }
}