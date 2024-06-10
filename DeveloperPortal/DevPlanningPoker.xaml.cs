using DeveloperPortal.Services;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class DevPlanningPoker
{
    public DevPlanningPoker(UserService userService, BaseHttpClientService baseHttpClientService)
    {
        InitializeComponent();
        BindingContext = new DevPlanningPokerViewModel(userService, baseHttpClientService);

        Appearing += async (_, _) => await ((DevPlanningPokerViewModel)BindingContext).LoadUsersAsync();
    }
}