using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Volo.DocsTestApp.EntityFrameworkCore
{
    public class DocsTestAppDbContextFactory : IDesignTimeDbContextFactory<DocsTestAppDbContext>
    {
        public DocsTestAppDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<DocsTestAppDbContext>()
                .UseSqlServer(configuration.GetConnectionString("SqlServer"));

            return new DocsTestAppDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Volo.DocsTestApp/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
