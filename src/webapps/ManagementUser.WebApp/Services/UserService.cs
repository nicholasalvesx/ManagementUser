using ManagementUser.WebApp.Models;
using Microsoft.AspNetCore.Identity;

namespace ManagementUser.WebApp.Services;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        var users = _userManager.Users.ToList();
        return users.Select(user => new User
        {
            Id = new Guid(user.Id),
            UserName = user.UserName,
            Email = user.Email
        }).ToList();
    }
    
    public async Task<bool> EditUserAsync(User model)
    {
        var user = await _userManager.FindByIdAsync(model.Id.ToString());
        if (user == null)
        {
            return false;
        }
        user.Email = model.Email;
        user.UserName = model.UserName;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }
}