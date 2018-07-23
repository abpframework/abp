using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<BackgroundJobOptions>>().Value;
            if (options.IsJobExecutionEnabled)
            {
                context.ServiceProvider
                    .GetRequiredService<IBackgroundWorkerManager>()
                    .Add(
                        context.ServiceProvider
                            .GetRequiredService<IBackgroundJobWorker>()
                    );
            }
        }
    }
}