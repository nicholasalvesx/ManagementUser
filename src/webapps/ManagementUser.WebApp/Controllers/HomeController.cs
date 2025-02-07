using ManagementUser.WebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ManagementUser.WebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace ManagementUser.WebApp.Controllers;

[Route("Home")]
public class HomeController(IdentityAppDbContext context, ILogger<HomeController> logger)
    : Controller
{
    [Authorize]
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var users = await context.Users.AsNoTracking().ToListAsync();
            return View(users);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao carregar os usuários.");
            return RedirectToAction(nameof(Error));
        }
  
    }
    [AllowAnonymous]
    [HttpGet("Error")]
    public IActionResult Error()
    {
        var errorModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };
        return View(errorModel);
    }
}