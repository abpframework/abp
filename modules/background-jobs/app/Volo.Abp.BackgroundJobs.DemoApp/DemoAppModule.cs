using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.DemoApp.Shared;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs.DemoApp
{
    [DependsOn(
        typeof(DemoAppSharedModule),
        typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
        typeof(AbpAutofacModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule)
        )]
    public class DemoAppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(opts =>
                {
                    opts.UseSqlServer();
                });
            });

            Configure<AbpBackgroundJobWorkerOptions>(options =>
            {
                //Configure for fast running
                options.JobPollPeriod = 1000;
                options.DefaultFirstWaitDuration = 1;
                options.DefaultWaitFactor = 1;
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            //TODO: Configure console logging
            //context
            //    .ServiceProvider
            //    .GetRequiredService<ILoggerFactory>()
            //    .AddConsole(LogLevel.Debug);
        }
    }
}
