using System.Globalization;
using DeveloperPortal.Authentication;
using DeveloperPortal.Services;
using DeveloperPortal.Services.Helpers;
using DeveloperPortal.Services.Interfaces;
using DeveloperPortal.ViewModels;
using IdentityModel.OidcClient;
using Microsoft.Extensions.Logging;

namespace DeveloperPortal;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // Set default culture before configuration starts
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

        builder
            .UseMauiApp<App>()
            .UseSentry(options =>
            {
                options.Dsn =
                    "https://7a8a2035ca3fc5642fc8bda4a76c866e@o4507341086064640.ingest.de.sentry.io/4507341088555088";
                options.Debug = true;
                options.TracesSampleRate = 1.0;
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register Auth0Client
        var oidcClient = new OidcClient(new()
        {
            Authority = $"https://{AuthenticationConstants.Auth0Domain}",
            ClientId = AuthenticationConstants.ClientId,
            RedirectUri = $"{AuthenticationConstants.AppProtocolName}://{AuthenticationConstants.AppCallbackUrl}/",
            PostLogoutRedirectUri = $"{AuthenticationConstants.AppProtocolName}://{AuthenticationConstants.AppCallbackUrl}/",
            Scope = string.Join(" ", AuthenticationConstants.Scopes),
            Browser = new AuthWorkaroundBrowser(),
        });
        ServiceLocator.AuthClient = oidcClient;
        builder.Services.AddSingleton(oidcClient);

        // Register services
        builder.Services.AddSingleton<IJiraService, JiraService>();
        builder.Services.AddSingleton<IAuth0ManagementService, Auth0ManagementService>();
        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<ISentryService, SentryService>();
        builder.Services.AddSingleton<IHttpHandler, DevHttpsConnectionHelper>(_ =>
            new DevHttpsConnectionHelper(7059));

        // Register view models
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<DevChatViewModel>();
        builder.Services.AddTransient<DevNotesViewModel>();
        builder.Services.AddTransient<DevPlanningPokerViewModel>();
        builder.Services.AddTransient<DevProfileViewModel>();
        builder.Services.AddTransient<JiraIssueViewModel>();
        builder.Services.AddTransient<SentryErrorViewModel>();
        builder.Services.AddTransient<MainPageViewModel>();

        // Register pages
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<Dashboard>();
        builder.Services.AddTransient<SentryErrors>();
        builder.Services.AddTransient<JiraIssues>();
        builder.Services.AddTransient<DevChat>();
        builder.Services.AddTransient<DevPlanningPoker>();
        builder.Services.AddTransient<DevNotes>();
        builder.Services.AddTransient<DevProfile>();
        

        // Register navigation service
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        
        builder.Services.AddSingleton<IServiceProvider>(builder.Services.BuildServiceProvider());

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        
        return app;
    }
}