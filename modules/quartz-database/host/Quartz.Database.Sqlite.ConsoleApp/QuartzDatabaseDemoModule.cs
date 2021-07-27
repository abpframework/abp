using Microsoft.Extensions.DependencyInjection;
using Quartz.Database.Sqlite.ConsoleApp.EntityFrameworkCore;
using QuartzDatabaseDemo;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;

namespace Quartz.Database.Sqlite.ConsoleApp
{

    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(QuartzDatabaseSharedDemoModule),
        typeof(AbpEntityFrameworkCoreSqliteModule)
    )]
    public class QuartzDatabaseDemoModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {

            PreConfigure<AbpQuartzOptions>(options =>
            {
                options.Configurator = configure =>
                {
                    configure.UsePersistentStore(storeOptions =>
                    {
                        storeOptions.UseProperties = true;
                        storeOptions.UseJsonSerializer();
                        storeOptions.UseSQLite("Data Source=QuartzDatabaseDemo.db");
                    });
                };
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<QuartzDatabaseDemoDbContext>();
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlite();
            });
        }
    }
}
