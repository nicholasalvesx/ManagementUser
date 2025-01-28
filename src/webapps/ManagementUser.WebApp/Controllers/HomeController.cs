using ManagementUser.WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ManagementUser.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IdentityAppDbContext _context;
        private readonly UserManager<IdentityUser<Guid>> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IdentityAppDbContext context, UserManager<IdentityUser<Guid>> userManager, ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;  
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar os usuários.");
                return View("Error"); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(string email, string password)
        {
            try
            {
                var user = new IdentityUser<Guid> { Email = email, UserName = email };
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Usuário {Email} criado com sucesso.", email);
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    _logger.LogWarning("Erro ao criar usuário {Email}: {Error}", email, error.Description);
                }

                return View("Index", await _context.Users.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar criar usuário com email {Email}.", email);
                return View("Error");
            }
        }
    }
}
