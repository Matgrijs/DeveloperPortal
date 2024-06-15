using DeveloperPortal.Services.Interfaces;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal;

public partial class SentryErrors
{
    public SentryErrors(ISentryService sentryService, IJiraService jiraService)
    {
        SentryErrorViewModel viewModel;
        InitializeComponent();

        BindingContext = viewModel = new SentryErrorViewModel(sentryService, jiraService);

        Appearing += async (_, _) => await viewModel.LoadErrors();
    }
}