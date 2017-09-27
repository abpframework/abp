using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Volo.Abp.TestApp.EntityFrameworkCore
{
    public class TestAppDbContextFactory : IDesignTimeDbContextFactory<TestAppDbContext>
    {
        public TestAppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TestAppDbContext>();
            builder.UseSqlite(@"Data Source=d:\temp\VoloAbpEfCoreTestModule.db;");
            return new TestAppDbContext(builder.Options);
        }
    }
}
