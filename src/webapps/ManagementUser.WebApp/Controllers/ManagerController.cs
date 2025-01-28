using ManagementUser.WebApp.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace ManagementUser.WebApp.Controllers;

public class UserManagementController(UserManager<IdentityUser<Guid>> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var users = userManager.Users.ToList();
        
        var userViewModels = users.Select(u => new UserViewModel
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email
        }).ToList();
        
        return View(userViewModels);
    }
}