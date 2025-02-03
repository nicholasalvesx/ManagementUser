using ManagementUser.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ManagementUser.WebApp.Controllers;

public class ManagerController : Controller
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;

    public ManagerController(UserManager<IdentityUser<Guid>> userManager)
    {
        _userManager = userManager;
    }

    public async Task Index()
    {
        await _userManager.Users.ToListAsync();
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }

        return View();
    }

    [HttpPost("edit")]
    public async Task<IActionResult> Edit(User model)
    {
        var user = await _userManager.FindByIdAsync(model.Id.ToString());
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
        return View("/Views/Manager/Edit");
    }
    
    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }
        return View();
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return RedirectToAction("Index");
        }

        ModelState.AddModelError(string.Empty, "Erro ao excluir o usuário.");
        return View("/Views/Manager/Delete");
    }

}
