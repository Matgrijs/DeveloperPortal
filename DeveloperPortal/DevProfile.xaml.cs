using DeveloperPortal.Services;

namespace DeveloperPortal;

public partial class DevProfile
{
    private readonly NavigationService _navigationService;
    public DevProfile(NavigationService navigationService)
    {
        InitializeComponent();
        
        _navigationService = navigationService;
    }

    private void OnAvansThemeClicked(object sender, EventArgs e)
    {
        // TODO implement
    }
    
    private void OnRoundedThemeClicked(object sender, EventArgs e)
    {
        // TODO implement
    }
    
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await _navigationService.OnBackButtonClickedAsync();
    }
}