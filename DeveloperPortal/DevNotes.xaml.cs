using DeveloperPortal.Services;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class DevNotes : ContentPage
{
    public DevNotes(ApiService apiService)
    {
        InitializeComponent();
        BindingContext = new DevNotesViewModel(apiService);

        Appearing += async (_, _) => await ((DevNotesViewModel)BindingContext).LoadNotesAsync();
    }
}