using ManagementUser.WebApp.Models;

namespace ManagementUser.WebApp.Services;

public interface IUserService
{
    Task<List<User>> GetAllUsersAsync();
    Task<bool> EditUserAsync(User model);
}