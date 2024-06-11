using DeveloperPortal.Services;
using DeveloperPortal.Services.DevHttpsConnectionHelper;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class DevPlanningPoker
{
    public DevPlanningPoker(UserService userService, IDevHttpsConnectionHelper httpsHelper)
    {
        InitializeComponent();
        BindingContext = new DevPlanningPokerViewModel(userService, httpsHelper);

        Appearing += async (_, _) => await ((DevPlanningPokerViewModel)BindingContext).LoadUsersAsync();
    }
}