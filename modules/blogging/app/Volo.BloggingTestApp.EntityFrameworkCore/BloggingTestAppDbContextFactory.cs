using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Volo.BloggingTestApp.EntityFrameworkCore
{
    public class BloggingTestAppDbContextFactory : IDesignTimeDbContextFactory<BloggingTestAppDbContext>
    {
        public BloggingTestAppDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<BloggingTestAppDbContext>()
                .UseSqlServer(configuration.GetConnectionString("SqlServer"));

            return new BloggingTestAppDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Volo.BloggingTestApp/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
