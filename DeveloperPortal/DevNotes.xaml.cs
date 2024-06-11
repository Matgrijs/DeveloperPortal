using DeveloperPortal.Services.DevHttpsConnectionHelper;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class DevNotes
{
    public DevNotes(IDevHttpsConnectionHelper httpsHelper)
    {
        InitializeComponent();
        BindingContext = new DevNotesViewModel(httpsHelper);

        Appearing += async (_, _) => await ((DevNotesViewModel)BindingContext).LoadNotesAsync();
    }
}