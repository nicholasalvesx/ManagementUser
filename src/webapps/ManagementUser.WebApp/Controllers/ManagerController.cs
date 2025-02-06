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

    [Authorize(Roles = "Admin")]
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
}