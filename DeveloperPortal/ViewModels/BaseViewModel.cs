using CommunityToolkit.Mvvm.ComponentModel;

namespace DeveloperPortal.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty] private bool _isBusy;

    [ObservableProperty] private string? _title;

    public bool IsNotBusy => !IsBusy;
}