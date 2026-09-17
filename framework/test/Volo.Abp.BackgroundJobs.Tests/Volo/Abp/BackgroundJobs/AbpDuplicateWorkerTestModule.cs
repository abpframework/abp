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
public class AbpDuplicateWorkerTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<WorkerStartRecorder>();
        context.Services.Replace(ServiceDescriptor.Transient<IBackgroundJobWorker, RecordingBackgroundJobWorker>());

        // The same job type is assigned to two dedicated workers, which must fail at initialization.
        Configure<AbpBackgroundJobWorkerOptions>(options =>
        {
            options.AddDedicatedWorker<WorkerJobAArgs>("lock-a");
            options.AddDedicatedWorker<WorkerJobAArgs>("lock-b");
        });
    }
}
