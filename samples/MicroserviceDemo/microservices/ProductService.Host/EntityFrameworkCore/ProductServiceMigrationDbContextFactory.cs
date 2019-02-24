using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProductService.Host.EntityFrameworkCore
{
    public class ProductServiceMigrationDbContextFactory : IDesignTimeDbContextFactory<ProductServiceMigrationDbContext>
    {
        public ProductServiceMigrationDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ProductServiceMigrationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("ProductManagement"));

            return new ProductServiceMigrationDbContext(builder.Options);
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
