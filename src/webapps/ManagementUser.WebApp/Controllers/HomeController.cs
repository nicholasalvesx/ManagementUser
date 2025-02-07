using ManagementUser.WebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ManagementUser.WebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace ManagementUser.WebApp.Controllers;

[Route("Home")]
public class HomeController : Controller
{
    private readonly IdentityAppDbContext _context;
    private readonly ILogger<HomeController> _logger;
   
    public HomeController(IdentityAppDbContext context, ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    [Authorize]
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();
            return View(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar os usuários.");
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