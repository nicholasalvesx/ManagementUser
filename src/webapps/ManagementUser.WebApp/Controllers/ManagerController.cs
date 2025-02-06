using ManagementUser.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ManagementUser.WebApp.Controllers;

[Authorize(Roles = "Admin")]
[Route("Manager")]
public class ManagerController : Controller
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;

    public ManagerController(UserManager<IdentityUser<Guid>> userManager)
    {
        _userManager = userManager;
    }
    
    [HttpGet("index")]
    public IActionResult Index()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [FromForm] IdentityUser<Guid> model)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }

        user.UserName = model.UserName;
        user.Email = model.Email;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return RedirectToAction("Index");
        }

        ModelState.AddModelError(string.Empty, "Erro ao atualizar o usuário.");
        return View(user);
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }
        return RedirectToAction("Index");
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }
   
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = new IdentityUser<Guid>
        {
            UserName = model.UserName,
            Email = model.Email, 
            EmailConfirmed = true
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(model);
    }
}