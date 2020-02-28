using System.Collections.Specialized;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.DemoApp.Shared;
using Volo.Abp.BackgroundJobs.Quartz;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;

namespace Volo.Abp.BackgroundJobs.DemoApp.Quartz
{
    [DependsOn(
        typeof(DemoAppSharedModule),
        typeof(AbpAutofacModule),
        typeof(AbpBackgroundJobsQuartzModule)
    )]
    public class DemoAppQuartzModule : AbpModule
    {
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
