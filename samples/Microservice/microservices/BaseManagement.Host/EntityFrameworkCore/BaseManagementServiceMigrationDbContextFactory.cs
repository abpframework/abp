using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BaseManagement.Host.EntityFrameworkCore
{
    public class BaseManagementServiceMigrationDbContextFactory : IDesignTimeDbContextFactory<BaseManagementServiceMigrationDbContext>
    {
        public BaseManagementServiceMigrationDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<BaseManagementServiceMigrationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("BaseManagement"));

            return new BaseManagementServiceMigrationDbContext(builder.Options);
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
