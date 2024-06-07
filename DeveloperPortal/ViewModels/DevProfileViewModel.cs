using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DeveloperPortal.ViewModels
{
    public partial class DevProfileViewModel : BaseViewModel
    {
        public DevProfileViewModel()
        {
            AvansThemeCommand = new RelayCommand(OnAvansThemeClicked);
            RoundedThemeCommand = new RelayCommand(OnRoundedThemeClicked);
        }

        public ICommand AvansThemeCommand { get; }
        public ICommand RoundedThemeCommand { get; }

        private void OnAvansThemeClicked()
        {
            // TODO: Implement Avans theme selection logic
        }

        private void OnRoundedThemeClicked()
        {
            // TODO: Implement Rounded theme selection logic
        }
    }
}