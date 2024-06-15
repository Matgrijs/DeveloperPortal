namespace DeveloperPortal.Services.Interfaces;

public interface IAuth0ManagementService
{
    Task<string> GetManagementApiToken();
}