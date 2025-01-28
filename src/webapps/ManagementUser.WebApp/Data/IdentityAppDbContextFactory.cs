using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ManagementUser.WebApp.Data
{
    public class IdentityAppDbContextFactory : IDesignTimeDbContextFactory<IdentityAppDbContext>
    {
        public IdentityAppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityAppDbContext>();
            optionsBuilder.UseSqlServer("DefaultConnection");

            return new IdentityAppDbContext(optionsBuilder.Options);
        }
    }
}