using System.Diagnostics;
using DeveloperPortal.Services;

namespace DeveloperPortal;

public partial class Dashboard
{
    public Dashboard()
    {
        InitializeComponent();
    }

    private async void OnNavigateClicked(object sender, EventArgs e)
    {
        if (!(sender is Button button)) return;
        var serviceProvider = App.Services;
        
        var sentryService = serviceProvider!.GetService<SentryService>();
        var jiraService = serviceProvider!.GetService<JiraService>();
        var userService = serviceProvider!.GetService<UserService>();
        var navigationService = serviceProvider!.GetService<NavigationService>();
        var apiService = serviceProvider!.GetService<ApiService>();

        if (navigationService == null || userService == null || sentryService == null || jiraService == null || apiService == null)
        {
            if (navigationService == null)
            {
                Debug.WriteLine("NavigationService");   
            }
            if (userService == null)
            {
                Debug.WriteLine("userService");   
            }
            if (sentryService == null)
            {
                Debug.WriteLine("sentryService");   
            }
            if (jiraService == null)
            {
                Debug.WriteLine("jiraService");   
            }
            if (apiService == null)
            {
                Debug.WriteLine("apiService");   
            }
            return;
        }
        
        switch (button.Text)
        {
            case "Planning poker":
                await Navigation.PushAsync(new DevPlanningPoker(userService));
                break;
            case "Notes":
                await Navigation.PushAsync(new DevNotes(navigationService, apiService));
                break;
            case "Sentry Errors":
                await Navigation.PushAsync(new SentryErrors(sentryService, jiraService, navigationService));
                break;
            case "Jira Issues":
                await Navigation.PushAsync(new JiraIssues(navigationService));
                break;
            case "Chat":
                await Navigation.PushAsync(new DevChat(navigationService, apiService));
                break;
            case "Profile":
                await Navigation.PushAsync(new DevProfile(navigationService));
                break;
        }
    }
}