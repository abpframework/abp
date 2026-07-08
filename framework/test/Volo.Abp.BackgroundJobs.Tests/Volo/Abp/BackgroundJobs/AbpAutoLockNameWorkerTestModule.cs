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
public class AbpAutoLockNameWorkerTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<WorkerStartRecorder>();
        context.Services.Replace(ServiceDescriptor.Transient<IBackgroundJobWorker, RecordingBackgroundJobWorker>());

        // No lock name is given: it is derived from the job argument type names.
        Configure<AbpBackgroundJobWorkerOptions>(options =>
        {
            options.AddDedicatedWorker<WorkerJobAArgs>();
            options.AddDedicatedWorker<WorkerJobBArgs>();
        });
    }
}
