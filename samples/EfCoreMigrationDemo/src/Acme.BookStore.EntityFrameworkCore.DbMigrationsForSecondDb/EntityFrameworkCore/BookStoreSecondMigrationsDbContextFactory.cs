using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Acme.BookStore.DbMigrationsForSecondDb.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class BookStoreSecondMigrationsDbContextFactory
        : IDesignTimeDbContextFactory<BookStoreSecondMigrationsDbContext>
    {
        public BookStoreSecondMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<BookStoreSecondMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("AbpPermissionManagement"));

            return new BookStoreSecondMigrationsDbContext(builder.Options);
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
