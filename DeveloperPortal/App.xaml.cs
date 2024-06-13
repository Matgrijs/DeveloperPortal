using Auth0.OidcClient;

namespace DeveloperPortal;

public partial class App
{
    public App(IServiceProvider? serviceProvider)
    {
        InitializeComponent();

        Services = serviceProvider;

        if (Services == null) return;
        var auth0Client = Services.GetRequiredService<Auth0Client>();

        MainPage = new NavigationPage(new MainPage(auth0Client));
    }

    public static IServiceProvider? Services { get; set; }
}