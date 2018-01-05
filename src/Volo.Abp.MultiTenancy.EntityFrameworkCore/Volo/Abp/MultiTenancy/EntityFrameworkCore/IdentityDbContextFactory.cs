using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Volo.Abp.MultiTenancy.EntityFrameworkCore
{
    /* This class is needed for EF Core command line tooling */

    public class MultiTenancyDbContextFactory : IDesignTimeDbContextFactory<MultiTenancyDbContext>
    {
        public MultiTenancyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MultiTenancyDbContext>();
            builder.UseSqlServer("Server=localhost;Database=AbpDesk;Trusted_Connection=True;");
            return new MultiTenancyDbContext(builder.Options);
        }
    }
}
