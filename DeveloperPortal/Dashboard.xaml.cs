using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class Dashboard
{
    public Dashboard()
    {
        InitializeComponent();

        var viewModel = App.Services!.GetService<DashboardViewModel>();
        BindingContext = viewModel ?? throw new InvalidOperationException("ViewModel not found.");
    }
}