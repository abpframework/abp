using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace QuartzDatabaseDemo.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class QuartzDatabaseDemoDbContextFactory : IDesignTimeDbContextFactory<QuartzDatabaseDemoDbContext>
    {
        public QuartzDatabaseDemoDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<QuartzDatabaseDemoDbContext>()
                .UseMySql(configuration.GetConnectionString("Default"), MySqlServerVersion.LatestSupportedServerVersion);

            return new QuartzDatabaseDemoDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Quartz.Database.MySql.ConsoleApp/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
