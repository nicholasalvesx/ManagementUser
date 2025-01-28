using ManagementUser.WebApp.ViewsModels;

namespace ManagementUser.WebApp.Services;

public interface IUserService
{
    Task<List<UserViewModel>> GetAllUsersAsync();
}