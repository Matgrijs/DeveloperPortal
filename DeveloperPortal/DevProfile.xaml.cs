using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class DevProfile : ContentPage
{
    public DevProfile()
    {
        InitializeComponent();
        BindingContext = new DevProfileViewModel();
    }

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        if (BindingContext is DevProfileViewModel vm) vm.ToggleLanguageCommand.Execute(null);
    }
}