using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace VoloDocs.EntityFrameworkCore
{
    public class VoloDocsDbContextFactory : IDesignTimeDbContextFactory<VoloDocsDbContext>
    {
        public VoloDocsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<VoloDocsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("SqlServer"));

            return new VoloDocsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../VoloDocs/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
