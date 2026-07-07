using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs;

[DependsOn(
    typeof(AbpBackgroundJobsModule),
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule)
)]
public class AbpDuplicateLockNameTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<WorkerStartRecorder>();
        context.Services.Replace(ServiceDescriptor.Transient<IBackgroundJobWorker, RecordingBackgroundJobWorker>());

        // Two workers with different job types but the same lock name, which must fail at initialization.
        Configure<AbpBackgroundJobWorkerOptions>(options =>
        {
            options.AddDedicatedWorker<WorkerJobAArgs>("dup-lock");
            options.AddDedicatedWorker<WorkerJobBArgs>("dup-lock");
        });
    }
}
