using DeveloperPortal.Services.Interfaces;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class DevNotes
{
    public DevNotes(IHttpHandler httpsHelper)
    {
        InitializeComponent();
        BindingContext = new DevNotesViewModel(httpsHelper);

        Appearing += async (_, _) => await ((DevNotesViewModel)BindingContext).LoadNotesAsync();
    }
}