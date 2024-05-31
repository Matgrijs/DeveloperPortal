using Microsoft.Extensions.Logging;
using Auth0.OidcClient;
using DeveloperPortal.Services;

namespace DeveloperPortal;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSentry(options => {
                // The DSN is the only required setting.
                options.Dsn = "https://7a8a2035ca3fc5642fc8bda4a76c866e@o4507341086064640.ingest.de.sentry.io/4507341088555088";

                // Use debug mode if you want to see what the SDK is doing.
                // Debug messages are written to stdout with Console.Writeline,
                // and are viewable in your IDE's debug console or with 'adb logcat', etc.
                // This option is not recommended when deploying your application.
                options.Debug = true;

                // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
                // We recommend adjusting this value in production.
                options.TracesSampleRate = 1.0;
                // Other Sentry options can be set here.
            })

            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
        builder.Services.AddSingleton<SentryService>(serviceProvider =>
        {
            var sentryApiToken = "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";
            var organization = "developerportalavans";
            var project = "dotnet-maui";
            return new SentryService(sentryApiToken, organization, project);
        });

        builder.Services.AddSingleton<JiraService>(serviceProvider => new JiraService());
        
        var auth0Client = new Auth0Client(new()
        {
            Domain = "developerportal.eu.auth0.com",
            ClientId = "dlRxoxG6hpTmFR6tGnNlhh8EX9bUd96d",
            RedirectUri = "myapp://callback/",
            PostLogoutRedirectUri = "myapp://callback/",
            Scope = "openid profile email"
        });
        
        builder.Services.AddSingleton(auth0Client);
        builder.Services.AddSingleton<Auth0ManagementService>();
        builder.Services.AddSingleton<NavigationService>();
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<UserService>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<Dashboard>();
        builder.Services.AddSingleton<DevChat>();
        builder.Services.AddSingleton<DevNotes>();
        builder.Services.AddSingleton<DevPlanningPoker>();
        
#if DEBUG
        builder.Logging.AddDebug();
#endif
        
        var app = builder.Build();
        
        // Set the service provider for use in App.xaml.cs
        App.Services = app.Services;

        return app;
    }
}