using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Volo.Abp.Permissions.EntityFrameworkCore
{
    /* This class is needed for EF Core command line tooling */

    public class AbpPermissionsDbContextFactory : IDesignTimeDbContextFactory<AbpPermissionsDbContext>
    {
        public AbpPermissionsDbContext CreateDbContext(string[] args)
        {
            //TODO: Remove all SqlServer references from EFCore packages and find a way of creating factory inside this.
            var builder = new DbContextOptionsBuilder<AbpPermissionsDbContext>();
            builder.UseSqlServer("Server=localhost;Database=AbpDesk;Trusted_Connection=True;");
            return new AbpPermissionsDbContext(builder.Options);
        }
    }
}
