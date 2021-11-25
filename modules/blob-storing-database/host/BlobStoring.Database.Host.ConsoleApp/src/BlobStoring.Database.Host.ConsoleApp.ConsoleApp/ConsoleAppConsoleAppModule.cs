using BlobStoring.Database.Host.ConsoleApp.ConsoleApp.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace BlobStoring.Database.Host.ConsoleApp.ConsoleApp;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(BlobStoringDatabaseEntityFrameworkCoreModule)
)]
public class ConsoleAppConsoleAppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureEntityFramework(context);

        context.Services.AddSingleton<IBlobProvider, DatabaseBlobProvider>();

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.ProviderType = typeof(DatabaseBlobProvider);
            });
        });
    }

    private void ConfigureEntityFramework(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = "Server=localhost;Database=BlobStoring_Host;Trusted_Connection=True";
        });

        context.Services.AddAbpDbContext<BlobStoringHostDbContext>(options =>
        {
            options.AddDefaultRepositories(true);
        });

        Configure<AbpDbContextOptions>(x =>
        {
            x.UseSqlServer();
        });
    }
}
