using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Hangfire;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs.Hangfire
{
    [DependsOn(
        typeof(AbpBackgroundJobsAbstractionsModule),
        typeof(AbpHangfireModule)
        )]
    public class AbpBackgroundJobsHangfireModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpBackgroundJobsHangfireModule>();
        }
    }
}
