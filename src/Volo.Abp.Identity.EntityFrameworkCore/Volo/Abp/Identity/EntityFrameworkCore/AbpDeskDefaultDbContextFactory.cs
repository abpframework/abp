using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace AbpDesk.EntityFrameworkCore
{
    /* This class is needed for EF Core command line tooling */

    public class IdentityDefaultDbContextFactory : IDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<IdentityDbContext>();
            builder.UseSqlServer("Server=localhost;Database=AbpDesk;Trusted_Connection=True;");
            return new IdentityDbContext(builder.Options);
        }
    }
}
