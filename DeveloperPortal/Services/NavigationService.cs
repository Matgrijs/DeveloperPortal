using System.Diagnostics;
using DeveloperPortal.Services.DevHttpsConnectionHelper;
using DeveloperPortal.Services.Interfaces;

namespace DeveloperPortal.Services
{
    public class NavigationService : INavigationService
    {
        private UserService _userService;
        private IDevHttpsConnectionHelper _devHttpsHelper;
        private SentryService _sentryService;
        private JiraService _jiraService;

        // Constructor with the necessary dependencies
        public NavigationService(
            UserService userService,
            IDevHttpsConnectionHelper devHttpsHelper,
            SentryService sentryService,
            JiraService jiraService)
        {
            _userService = userService;
            _devHttpsHelper = devHttpsHelper;
            _sentryService = sentryService;
            _jiraService = jiraService;
        }

        public async Task NavigateToAsync(string destination)
        {
            Debug.WriteLine($"destination 2: {destination}");
            Page? page = destination switch
            {
                "Planning poker" => new DevPlanningPoker(_userService, _devHttpsHelper),
                "Notes" => new DevNotes(_devHttpsHelper),
                "Sentry Errors" => new SentryErrors(_sentryService, _jiraService),
                "Jira Issues" => new JiraIssues(_jiraService),
                "Chat" => new DevChat(_devHttpsHelper),
                "Profile" => new DevProfile(),
                "Dashboard" => new Dashboard(this),
                _ => null
            };

            if (page != null)
                Debug.WriteLine($"destination 3: {destination}");
            if (Application.Current != null)
                Debug.WriteLine($"destination 4: {destination}");
            if (Application.Current.MainPage != null)
                Debug.WriteLine($"destination 5: {destination}");
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}