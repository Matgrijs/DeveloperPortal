using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class MainPage: ContentPage
{
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }
    
}