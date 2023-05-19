using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BlobStoring.Database.Host.ConsoleApp.ConsoleApp.EfCore;

public class BlobStoringHostDbContextFactory : IDesignTimeDbContextFactory<BlobStoringHostDbContext>
{
    public BlobStoringHostDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<BlobStoringHostDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new BlobStoringHostDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
