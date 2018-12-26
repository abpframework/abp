using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Volo.AbpWebSite.EntityFrameworkCore
{
    public class AbpWebSiteDbContextFactory : IDesignTimeDbContextFactory<AbpWebSiteDbContext>
    {
        public AbpWebSiteDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<AbpWebSiteDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new AbpWebSiteDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Volo.AbpWebSite.Web/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}