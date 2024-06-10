using DeveloperPortal.Services;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class DevNotes : ContentPage
{
    public DevNotes(BaseHttpClientService baseHttpClientService)
    {
        InitializeComponent();
        BindingContext = new DevNotesViewModel(baseHttpClientService);

        Appearing += async (_, _) => await ((DevNotesViewModel)BindingContext).LoadNotesAsync();
    }
}