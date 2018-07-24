using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs
{
    [DependsOn(
        typeof(AbpJsonModule)
        )]
    public class AbpBackgroundJobsAbstractionsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpBackgroundJobsAbstractionsModule>();
        }
    }
}
