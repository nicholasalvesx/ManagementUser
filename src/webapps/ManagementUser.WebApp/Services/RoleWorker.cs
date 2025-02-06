using Microsoft.AspNetCore.Identity;

namespace ManagementUser.WebApp.Services
{
    public class RoleWorker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RoleWorker> _logger;

        public RoleWorker(IServiceProvider serviceProvider, ILogger<RoleWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RoleWorker iniciado");

            using var scope = _serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser<Guid>>>();

            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    _logger.LogInformation($"Criando a role: {role}");
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            var adminEmail = "admin@admin.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var user = new IdentityUser<Guid>
                {
                    UserName = "Admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "@Admin123");
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Criando usuário: {adminEmail}");
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            _logger.LogInformation("RoleWorker finalizado");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RoleWorker foi parado");
            return Task.CompletedTask;
        }
    }
}
