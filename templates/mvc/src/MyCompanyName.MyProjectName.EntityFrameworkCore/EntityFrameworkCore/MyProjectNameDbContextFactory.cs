using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    public class MyProjectNameDbContextFactory : IDesignTimeDbContextFactory<MyProjectNameDbContext>
    {
        public MyProjectNameDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<MyProjectNameDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new MyProjectNameDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../MyCompanyName.MyProjectName.Web/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
