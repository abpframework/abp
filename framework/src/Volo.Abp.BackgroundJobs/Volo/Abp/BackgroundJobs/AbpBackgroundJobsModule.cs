using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Guids;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs
{
    [DependsOn(
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpTimingModule),
        typeof(AbpJsonModule),
        typeof(AbpGuidsModule)
        )]
    public class AbpBackgroundJobsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpBackgroundJobsModule>();
        }
    }
}