using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Volo.Abp.EntityFrameworkCore.TestApp.SecondContext
{
    public class SecondDbContextFactory : IDesignTimeDbContextFactory<SecondDbContext>
    {
        public SecondDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SecondDbContext>();
            builder.UseSqlite(@"Data Source=d:\temp\VoloAbpEfCoreTestModule.db;");
            return new SecondDbContext(builder.Options);
        }
    }
}
