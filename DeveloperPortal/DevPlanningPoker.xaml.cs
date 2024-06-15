using DeveloperPortal.Services.Interfaces;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class DevPlanningPoker
{
    public DevPlanningPoker(IUserService userService, IHttpHandler httpsHelper)
    {
        InitializeComponent();
        BindingContext = new DevPlanningPokerViewModel(userService, httpsHelper);

        Appearing += async (_, _) => await ((DevPlanningPokerViewModel)BindingContext).LoadUsersAsync();
    }
}