using DeveloperPortal.Services.Interfaces;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class Dashboard
{
    public Dashboard(INavigationService navigationService)
    {
        InitializeComponent();

        var viewModel = new DashboardViewModel(navigationService);
        BindingContext = viewModel;
    }
}