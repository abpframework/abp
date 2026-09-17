using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class MyProjectNameDbContextFactory : IDesignTimeDbContextFactory<MyProjectNameDbContext>
{
    public MyProjectNameDbContext CreateDbContext(string[] args)
    {
//<TEMPLATE-REMOVE IF-NOT='dbms:PostgreSQL'>
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//</TEMPLATE-REMOVE>
        MyProjectNameEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<MyProjectNameDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new MyProjectNameDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(GetDbMigratorProjectFolderPath())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }

    private static string GetDbMigratorProjectFolderPath()
    {
        var slnDirectoryPath = GetSolutionDirectoryPath();

        if (string.IsNullOrEmpty(slnDirectoryPath))
        {
            throw new Exception("Solution folder not found!");
        }

        var srcDirectoryPath = Path.Combine(slnDirectoryPath, "src");

        return Directory.GetDirectories(srcDirectoryPath)
            .FirstOrDefault(d => d.EndsWith(".DbMigrator")) ?? string.Empty;
    }

    private static string GetSolutionDirectoryPath()
    {
        var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (currentDirectory != null && Directory.GetParent(currentDirectory.FullName) != null)
        {
            currentDirectory = Directory.GetParent(currentDirectory.FullName);

            if (currentDirectory != null && Directory.GetFiles(currentDirectory.FullName).FirstOrDefault(f => f.EndsWith(".sln") || f.EndsWith(".slnx")) != null)
            {
                return currentDirectory.FullName;
            }
        }

        return string.Empty;
    }
}
