using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace DeveloperPortal.Services;

public class NavigationService
{
    public async Task OnBackButtonClickedAsync()
    {
        await Application.Current.MainPage.Navigation.PopAsync();
    }
}