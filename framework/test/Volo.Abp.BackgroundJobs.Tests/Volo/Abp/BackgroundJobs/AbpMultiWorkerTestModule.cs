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
public class AbpMultiWorkerTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<WorkerStartRecorder>();

        // Replace the real worker with a recording one to assert how the manager resolves and starts workers.
        context.Services.Replace(ServiceDescriptor.Transient<IBackgroundJobWorker, RecordingBackgroundJobWorker>());

        Configure<AbpBackgroundJobWorkerOptions>(options =>
        {
            options.AddDedicatedWorker<WorkerJobAArgs>("lock-a");
            options.AddDedicatedWorker<WorkerJobBArgs>("lock-b");
        });
    }
}
