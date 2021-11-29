using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.DemoApp.Shared;
using Volo.Abp.Modularity;
using Microsoft.Extensions.Configuration;
using Volo.Abp.BackgroundJobs.Hangfire;

namespace Volo.Abp.BackgroundJobs.DemoApp.HangFire
{
    [DependsOn(
        typeof(DemoAppSharedModule),
        typeof(AbpAutofacModule),
        typeof(AbpBackgroundJobsHangfireModule)
    )]
    public class DemoAppHangfireModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            context.Services.PreConfigure<IGlobalConfiguration>(hangfireConfiguration =>
            {
                hangfireConfiguration.UseSqlServerStorage(configuration.GetConnectionString("Default"));
            });
        }
    }
}
