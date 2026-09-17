using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs;

[DependsOn(
    typeof(AbpBackgroundJobsModule),
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule)
)]
public class AbpBackgroundJobWorkerTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Drive the worker manually in tests; don't run the real periodic worker.
        Configure<AbpBackgroundJobOptions>(options =>
        {
            options.IsJobExecutionEnabled = false;
        });

        context.Services.AddSingleton<ParallelJobTracker>();
        context.Services.AddScoped<ScopeMarker>();
    }
}
