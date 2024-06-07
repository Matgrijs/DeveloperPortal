using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class DevProfile : ContentPage
{
    public DevProfile()
    {
        InitializeComponent();
        BindingContext = new DevProfileViewModel();
    }
}