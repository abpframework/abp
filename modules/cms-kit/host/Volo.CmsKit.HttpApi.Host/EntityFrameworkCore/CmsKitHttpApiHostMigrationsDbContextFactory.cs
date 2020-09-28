using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Volo.CmsKit.EntityFrameworkCore
{
    public class CmsKitHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<CmsKitHttpApiHostMigrationsDbContext>
    {
        public CmsKitHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            FeatureConfigurer.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<CmsKitHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("CmsKit"));

            return new CmsKitHttpApiHostMigrationsDbContext(builder.Options);
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
