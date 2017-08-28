using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace AbpDesk.EntityFrameworkCore
{
    /* This class is needed for EF Core command line tooling */

    public class IdentityDefaultDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<IdentityDbContext>();
            builder.UseSqlServer("Server=localhost;Database=AbpDesk;Trusted_Connection=True;");
            return new IdentityDbContext(builder.Options);
        }
    }
}
