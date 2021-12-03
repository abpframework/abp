using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore;

public class MyProjectNameHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<MyProjectNameHttpApiHostMigrationsDbContext>
{
    public MyProjectNameHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<MyProjectNameHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("MyProjectName"));

        return new MyProjectNameHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
