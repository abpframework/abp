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
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../MyCompanyName.MyProjectName.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
