namespace DeveloperPortal.Services.Navigation;

public interface INavigationService
{
    Task NavigateToAsync(string destination);
}