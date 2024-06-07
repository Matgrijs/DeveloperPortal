using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DeveloperPortal.Services.Navigation;

namespace DeveloperPortal.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        public ICommand NavigateCommand { get; }

        public DashboardViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            Title = "Dashboard";
            NavigateCommand = new RelayCommand<string>(OnNavigate);
        }

        private async void OnNavigate(string? destination)
        {
            if (destination != null)
            {
                await _navigationService.NavigateToAsync(destination);
            }
        }
    }
}