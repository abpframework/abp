using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.DemoApp.Shared;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs.DemoApp
{
    [DependsOn(
        typeof(DemoAppSharedModule),
        typeof(BackgroundJobsEntityFrameworkCoreModule),
        typeof(AbpAutofacModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule)
        )]
    public class DemoAppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = ConfigurationHelper.BuildConfiguration();

            context.Services.AddConfiguration(configuration);

            context.Services.Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = configuration.GetConnectionString("Default");
            });

            context.Services.Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(opts =>
                {
                    opts.UseSqlServer();
                });
            });

            context.Services.Configure<BackgroundJobWorkerOptions>(options =>
            {
                //Configure for fast running
                options.JobPollPeriod = 1000;
                options.DefaultFirstWaitDuration = 1;
                options.DefaultWaitFactor = 1;
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context
                .ServiceProvider
                .GetRequiredService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);
        }
    }
}
