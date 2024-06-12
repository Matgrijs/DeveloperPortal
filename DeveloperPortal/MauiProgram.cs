using Microsoft.Extensions.Logging;
using Auth0.OidcClient;
using DeveloperPortal.Services;
using DeveloperPortal.Services.DevHttpsConnectionHelper;
using DeveloperPortal.Services.Navigation;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        // Set default culture before configuration starts
        System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("en-US");
        System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo("en-US");
        
        builder
            .UseMauiApp<App>()
            .UseSentry(options => {
                options.Dsn = "https://7a8a2035ca3fc5642fc8bda4a76c866e@o4507341086064640.ingest.de.sentry.io/4507341088555088";
                options.Debug = true;
                options.TracesSampleRate = 1.0;
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register Auth0Client
        var auth0Client = new Auth0Client(new()
        {
            Domain = "developerportal.eu.auth0.com",
            ClientId = "dlRxoxG6hpTmFR6tGnNlhh8EX9bUd96d",
            RedirectUri = "myapp://callback/",
            PostLogoutRedirectUri = "myapp://callback/",
            Scope = "openid profile email"
        });
        builder.Services.AddSingleton(auth0Client);

        // Register services
        builder.Services.AddSingleton<JiraService>();
        builder.Services.AddSingleton<Auth0ManagementService>();
        builder.Services.AddSingleton<UserService>();
        builder.Services.AddSingleton<SentryService>();
        builder.Services.AddSingleton<IDevHttpsConnectionHelper, DevHttpsConnectionHelper>(provider => new DevHttpsConnectionHelper(7059));

        // Register view models
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<DevChatViewModel>();
        builder.Services.AddTransient<DevNotesViewModel>();
        builder.Services.AddTransient<DevPlanningPokerViewModel>();
        builder.Services.AddTransient<DevProfileViewModel>();
        builder.Services.AddTransient<JiraIssueViewModel>();
        builder.Services.AddTransient<SentryErrorViewModel>();

        // Register pages
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<Dashboard>();
        builder.Services.AddTransient<DevChat>();
        builder.Services.AddTransient<DevNotes>();
        builder.Services.AddTransient<DevPlanningPoker>();

        // Register navigation service
        builder.Services.AddSingleton<INavigationService, NavigationService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        
        // Set the service provider for use in App.xaml.cs
        App.Services = app.Services;

        return app;
    }
}
