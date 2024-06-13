using DeveloperPortal.Services.Helpers;
using DeveloperPortal.Services.Interfaces;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class MainPage: ContentPage, INavigationService
{
    public MainPage()
    {
        InitializeComponent();
        
        var viewModel = new MainPageViewModel(ServiceLocator.AuthClient, this);
        BindingContext = viewModel;
    }

    public async Task NavigateToAsync(string route)
    {
        switch (route)
        {
            case "Dashboard":
                await Navigation.PushAsync(new Dashboard(this));
                break;
        }
    }
    
}