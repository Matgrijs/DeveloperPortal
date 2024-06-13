using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using User = DeveloperPortal.Models.Users.User;

namespace DeveloperPortal.Services;

public class UserService
{
    private readonly Auth0ManagementService _auth0ManagementServiceService;
    private readonly string _domain = "developerportal.eu.auth0.com";

    public UserService(Auth0ManagementService auth0ManagementServiceService)
    {
        _auth0ManagementServiceService = auth0ManagementServiceService;
    }

    public async Task<IList<User>> GetUsersAsync()
    {
        try
        {
            var token = await _auth0ManagementServiceService.GetManagementApiToken();
            if (string.IsNullOrEmpty(token)) throw new Exception("Access token is missing.");

            var managementApiClient = new ManagementApiClient(token, new Uri($"https://{_domain}/api/v2/"));
            var users = await managementApiClient.Users.GetAllAsync(new GetUsersRequest());
            return users.Select(u => new User
            {
                Name = u.FullName,
                Auth0Id = u.UserId
            }).ToList();
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
            throw;
        }
    }
}