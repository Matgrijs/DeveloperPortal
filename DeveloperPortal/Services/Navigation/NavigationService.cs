namespace DeveloperPortal.Services.Navigation;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task NavigateToAsync(string destination)
    {
        Page? page = destination switch
        {
            "Planning poker" => new DevPlanningPoker(_serviceProvider.GetRequiredService<UserService>(), _serviceProvider.GetRequiredService<BaseHttpClientService>()),
            "Notes" => new DevNotes(_serviceProvider.GetRequiredService<BaseHttpClientService>()),
            "Sentry Errors" => new SentryErrors(_serviceProvider.GetRequiredService<SentryService>(), _serviceProvider.GetRequiredService<JiraService>()),
            "Jira Issues" => new JiraIssues(_serviceProvider.GetRequiredService<JiraService>()),
            "Chat" => new DevChat(_serviceProvider.GetRequiredService<BaseHttpClientService>()),
            "Profile" => new DevProfile(),
            _ => null
        };

        if (page != null)
        {
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}
