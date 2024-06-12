using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Globalization;
using System.Resources;
using DeveloperPortal.Resources;
using DeveloperPortal.Services;

namespace DeveloperPortal.ViewModels
{
    public partial class DevProfileViewModel : ObservableRecipient
    {
        private readonly ResourceManager _resourceManager = new ResourceManager(typeof(AppResources));

        [ObservableProperty]
        private bool isDutchEnabled = CultureHelper.IsDutchEnabled;

        public string AdjustLanguageLabel => _resourceManager.GetString("AdjustLanguage", CultureInfo.CurrentCulture);
        public string TitleLabel => _resourceManager.GetString("ProfileTitle", CultureInfo.CurrentCulture);
        
        public DevProfileViewModel()
        {
            IsActive = true;
            // Register message listener
            Messenger.Register<DevProfileViewModel, LanguageChangedMessage>(this, (recipient, message) =>
            {
                if (CultureInfo.CurrentCulture.Name != message.NewCulture)
                {
                    CultureHelper.CurrentCultureCode = message.NewCulture; // Update culture via helper
                    recipient.IsDutchEnabled = CultureHelper.IsDutchEnabled;
                    recipient.OnPropertyChanged(nameof(AdjustLanguageLabel));
                    recipient.OnPropertyChanged(nameof(TitleLabel));
                }
            });
        }

        [RelayCommand]
        private void ToggleLanguage()
        {
            CultureHelper.CurrentCultureCode = IsDutchEnabled ? "nl-NL" : "en-US";
            OnPropertyChanged(nameof(AdjustLanguageLabel));
            OnPropertyChanged(nameof(TitleLabel));
            OnPropertyChanged(nameof(IsDutchEnabled));
            // Send a message to notify other parts of the app of the culture change
            Messenger.Send(new LanguageChangedMessage(CultureHelper.CurrentCultureCode));
        }

        protected override void OnActivated()
        {
            IsDutchEnabled = CultureHelper.IsDutchEnabled;  // Ensure the toggle reflects the current culture setting
        }
    }
}
