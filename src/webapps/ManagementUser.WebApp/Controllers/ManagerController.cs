using ManagementUser.WebApp.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace ManagementUser.WebApp.Controllers;

public class UserManagementController : Controller
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;

    public UserManagementController(UserManager<IdentityUser<Guid>> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        // Obtendo todos os usuários do banco
        var users = _userManager.Users.ToList();

        // Convertendo para UserViewModel
        var userViewModels = users.Select(u => new UserViewModel
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email
        }).ToList();

        // Passando para a View
        return View(userViewModels);
    }
}