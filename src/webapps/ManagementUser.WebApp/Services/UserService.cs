using ManagementUser.WebApp.ViewsModels;
using Microsoft.AspNetCore.Identity;

namespace ManagementUser.WebApp.Services;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<UserViewModel>> GetAllUsersAsync()
    {
        var users = _userManager.Users.ToList();
        return users.Select(user => new UserViewModel
        {
            Id = new Guid(user.Id),
            UserName = user.UserName,
            Email = user.Email
        }).ToList();
    }
}