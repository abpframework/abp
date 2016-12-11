using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AbpDesk.EntityFrameworkCore
{
    /* This class is needed for EF Core command line tooling */

    public class AbpDeskDefaultDbContextFactory : IDbContextFactory<AbpDeskDbContext>
    {
        public AbpDeskDbContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<AbpDeskDbContext>();
            builder.UseSqlServer("Server=localhost;Database=AbpDesk;Trusted_Connection=True;");
            return new AbpDeskDbContext(builder.Options);
        }
    }
}
