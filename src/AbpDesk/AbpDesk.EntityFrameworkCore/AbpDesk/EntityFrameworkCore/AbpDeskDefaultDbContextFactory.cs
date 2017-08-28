using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AbpDesk.EntityFrameworkCore
{
    /* This class is needed for EF Core command line tooling */

    public class AbpDeskDefaultDbContextFactory : IDesignTimeDbContextFactory<AbpDeskDbContext>
    {
        public AbpDeskDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AbpDeskDbContext>();
            builder.UseSqlServer("Server=localhost;Database=AbpDesk;Trusted_Connection=True;");
            return new AbpDeskDbContext(builder.Options);
        }
    }
}
