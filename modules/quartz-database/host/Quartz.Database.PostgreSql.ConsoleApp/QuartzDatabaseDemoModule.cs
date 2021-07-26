using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using QuartzDatabaseDemo.EntityFrameworkCore;
using System;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;

namespace QuartzDatabaseDemo
{

    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(QuartzDatabaseSharedDemoModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
    public class QuartzDatabaseDemoModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            PreConfigure<AbpQuartzOptions>(options =>
            {
                options.Properties["quartz.scheduler.instanceId"] = "AUTO";
                options.Properties["quartz.jobStore.clustered"] = "true";
                options.Configurator = configure =>
                {
                    configure.UsePersistentStore(storeOptions =>
                    {
                        storeOptions.UseProperties = true;
                        storeOptions.UseJsonSerializer();
                        storeOptions.UsePostgres(configuration.GetConnectionString("Default"));
                        storeOptions.UseClustering(c =>
                        {
                            c.CheckinMisfireThreshold = TimeSpan.FromSeconds(20);
                            c.CheckinInterval = TimeSpan.FromSeconds(10);
                        });
                    });
                };
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<QuartzDatabaseDemoDbContext>();
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
