using System.Diagnostics;
using DeveloperPortal.Services.Interfaces;

namespace DeveloperPortal.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        // Constructor with the necessary dependencies
        public NavigationService(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task NavigateToAsync(string destination)
        {
            Page? page = destination switch
            {
                "Planning poker" => _serviceProvider.GetService<DevPlanningPoker>(),
                "Notes"=> _serviceProvider.GetService<DevNotes>(),
                "Sentry Errors" => _serviceProvider.GetService<SentryErrors>(),
                "Jira Issues" => _serviceProvider.GetService<JiraIssues>(),
                "Chat" => _serviceProvider.GetService<DevChat>(),
                "Profile" => _serviceProvider.GetService<DevProfile>(),
                "Dashboard" => _serviceProvider.GetService<Dashboard>(),
                _ => null
            };
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}