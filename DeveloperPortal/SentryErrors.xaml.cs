using DeveloperPortal.Services;
using DeveloperPortal.ViewModels;

namespace DeveloperPortal
{
    public partial class SentryErrors
    {
        public SentryErrors(SentryService sentryService, JiraService jiraService)
        {
            SentryErrorViewModel viewModel;
            InitializeComponent();

            BindingContext = viewModel = new SentryErrorViewModel(sentryService, jiraService);

            Appearing += async (_, _) => await viewModel.LoadErrors();
        }
    }
}