using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace OrganizationService.Host.EntityFrameworkCore
{
    public class OrganizationServiceServiceMigrationDbContextFactory : IDesignTimeDbContextFactory<OrganizationServiceServiceMigrationDbContext>
    {
        public OrganizationServiceServiceMigrationDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<OrganizationServiceServiceMigrationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("OrganizationService"));

            return new OrganizationServiceServiceMigrationDbContext(builder.Options);
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
