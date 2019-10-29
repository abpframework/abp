using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using VoloDocs.EntityFrameworkCore;

namespace VoloDocs.Migrator
{
    [DependsOn(typeof(VoloDocsEntityFrameworkCoreModule))]
    public class VoloDocsMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            context.Services.AddAbpDbContext<VoloDocsDbContext>();

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = configuration["ConnectionString"];
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });
        }
    }
}