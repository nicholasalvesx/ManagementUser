using ManagementUser.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ManagementUser.WebApp.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly SignInManager<IdentityUser<Guid>> _signInManager;

    public AuthController(UserManager<IdentityUser<Guid>> userManager, SignInManager<IdentityUser<Guid>> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }
   
    [HttpPost("login")]
    public async Task<IActionResult> Login(User model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (model.Email != null)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is { UserName: not null })
            {
                if (model.Password != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

                    if (result.Succeeded)
                        return RedirectToAction("Index", "Home");
                }
            }
        }

        ModelState.AddModelError(string.Empty, "Login inválido.");
        return View(model);
    }

    [HttpGet("register")]
    public async Task<IActionResult> Register()
    {
        await _signInManager.SignOutAsync();
        return View();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User model)
    {
        if (!ModelState.IsValid)
            return View();

        var user = new IdentityUser<Guid>
        {
            UserName = model.UserName?.Split('@')[0],
            Email = model.Email
        };

        if (model.Password == null) return View(model);
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
                
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Login", "Auth"); 
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Auth");
    }

}