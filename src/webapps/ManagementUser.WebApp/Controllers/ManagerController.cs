using ManagementUser.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ManagementUser.WebApp.Controllers;

public class UserManagementController : Controller
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;

    public UserManagementController(UserManager<IdentityUser<Guid>> userManager)
    {
        _userManager = userManager;
    }

    public async Task Index()
    {
        await _userManager.Users.ToListAsync();
    }
}