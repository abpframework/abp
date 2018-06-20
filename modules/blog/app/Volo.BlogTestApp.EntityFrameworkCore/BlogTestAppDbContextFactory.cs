using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Volo.BlogTestApp.EntityFrameworkCore
{
    public class BlogTestAppDbContextFactory : IDesignTimeDbContextFactory<BlogTestAppDbContext>
    {
        public BlogTestAppDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<BlogTestAppDbContext>()
                .UseSqlServer(configuration.GetConnectionString("SqlServer"));

            return new BlogTestAppDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Volo.BlogTestApp/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
