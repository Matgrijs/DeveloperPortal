using DeveloperPortal.Services.DevHttpsConnectionHelper;
using DeveloperPortal.Services.Interfaces;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class DevPlanningPoker
{
    public DevPlanningPoker(IUserService userService, IDevHttpsConnectionHelper httpsHelper)
    {
        InitializeComponent();
        BindingContext = new DevPlanningPokerViewModel(userService, httpsHelper);

        Appearing += async (_, _) => await ((DevPlanningPokerViewModel)BindingContext).LoadUsersAsync();
    }
}