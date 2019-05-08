using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IdentityServerHost
{
    public class DemoAppDbContextFactory : IDesignTimeDbContextFactory<DemoAppDbContext>
    {
        public DemoAppDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<DemoAppDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new DemoAppDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
