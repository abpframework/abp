using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class MyProjectNameMigrationsDbContextFactory : IDesignTimeDbContextFactory<MyProjectNameMigrationsDbContext>
    {
        public MyProjectNameMigrationsDbContext CreateDbContext(string[] args)
        {
            MyProjectNameEfCoreEntityExtensionMappings.Configure();

            var application = AbpApplicationFactory.Create<MyProjectNameEntityFrameworkCoreDbMigrationsModule>(
                options =>
                {
                    options.UseAutofac();
                });

            application.Services.ReplaceConfiguration(BuildConfiguration());

            application.Services.AddTransient(provider =>
                new DbContextOptionsBuilder<MyProjectNameMigrationsDbContext>()
                    .UseSqlServer(provider.GetRequiredService<IConfiguration>().GetConnectionString("Default")).Options);

            application.Initialize();

            return application.ServiceProvider.GetRequiredService<MyProjectNameMigrationsDbContext>();
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../MyCompanyName.MyProjectName.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
