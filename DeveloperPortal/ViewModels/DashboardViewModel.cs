using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Globalization;
using System.Resources;
using System.Windows.Input;
using DeveloperPortal.Resources;
using DeveloperPortal.Services;
using DeveloperPortal.Services.Navigation;

namespace DeveloperPortal.ViewModels
{
    public partial class DashboardViewModel : ObservableRecipient
    {
        private readonly INavigationService _navigationService;
        private readonly ResourceManager _resourceManager = new (typeof(AppResources));
        
        [ObservableProperty]
        private string profileLabel;
        [ObservableProperty]
        private string chatLabel;
        [ObservableProperty]
        private string notesLabel;

        public ICommand NavigateCommand { get; }

        public DashboardViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateCommand = new RelayCommand<string>(OnNavigate);
            IsActive = true;
            UpdateLabels();

            // Listen for culture changes
            Messenger.Register<DashboardViewModel, LanguageChangedMessage>(this, (recipient, message) =>
            {
                recipient.UpdateLabels();
            });
        }

        private void UpdateLabels()
        {
            ProfileLabel = _resourceManager.GetString("ProfileTitle", CultureInfo.CurrentCulture);
            ChatLabel = _resourceManager.GetString("ChatTitle", CultureInfo.CurrentCulture);
            NotesLabel = _resourceManager.GetString("NotesTitle", CultureInfo.CurrentCulture);
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