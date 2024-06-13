using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DeveloperPortal.Models;
using DeveloperPortal.Resources;
using DeveloperPortal.Services.Interfaces;

namespace DeveloperPortal.ViewModels;

public partial class DashboardViewModel : ObservableRecipient
{
    private readonly INavigationService _navigationService;
    private readonly ResourceManager _resourceManager = new(typeof(AppResources));

    [ObservableProperty] private string? _chatLabel;

    [ObservableProperty] private string? _notesLabel;

    [ObservableProperty] private string? _profileLabel;

    public DashboardViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        NavigateCommand = new RelayCommand<string>(OnNavigate);
        IsActive = true;
        UpdateLabels();

        // Listen for culture changes
        Messenger.Register<DashboardViewModel, LanguageChangedMessage>(this,
            (recipient, _) => { recipient.UpdateLabels(); });
    }

    public ICommand NavigateCommand { get; }

    private void UpdateLabels()
    {
        ProfileLabel = _resourceManager.GetString("ProfileTitle", CultureInfo.CurrentCulture);
        ChatLabel = _resourceManager.GetString("ChatTitle", CultureInfo.CurrentCulture);
        NotesLabel = _resourceManager.GetString("NotesTitle", CultureInfo.CurrentCulture);
    }

    public async void OnNavigate(string? destination)
    {
        Debug.WriteLine($"destination: {destination}");
        if (destination != null) await _navigationService.NavigateToAsync(destination);
    }
}