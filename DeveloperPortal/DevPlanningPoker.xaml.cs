using DeveloperPortal.Services;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class DevPlanningPoker
{
    public DevPlanningPoker(UserService userService)
    {
        InitializeComponent();
        BindingContext = new DevPlanningPokerViewModel(userService);

        Appearing += async (_, _) => await ((DevPlanningPokerViewModel)BindingContext).LoadUsersAsync();
    }
}