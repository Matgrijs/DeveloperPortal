namespace DeveloperPortal.Services.Interfaces;

public interface INavigationService
{
    Task NavigateToAsync(string destination);
}