using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore;

public class OpenIddictHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<OpenIddictHostMigrationsDbContext>
{
    public OpenIddictHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<OpenIddictHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new OpenIddictHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
