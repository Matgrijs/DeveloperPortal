using DeveloperPortal.Models.Users;

namespace DeveloperPortal.Services.Interfaces
{
    public interface IUserService
    {
        Task<IList<User>> GetUsersAsync();
    }
}