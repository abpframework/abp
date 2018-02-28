using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MicroserviceDemo.AuthServer.Db
{
    /* This class is needed for EF Core command line tooling */

    public class MigrationDbContextFactory : IDesignTimeDbContextFactory<MigrationDbContext>
    {
        public MigrationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MigrationDbContext>();
            builder.UseSqlServer("Server=localhost;Database=MicroservicesDemo.Web;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new MigrationDbContext(builder.Options);
        }
    }
}
