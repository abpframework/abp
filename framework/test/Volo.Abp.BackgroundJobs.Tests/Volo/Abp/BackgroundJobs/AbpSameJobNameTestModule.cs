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
public class AbpSameJobNameTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<WorkerStartRecorder>();
        context.Services.Replace(ServiceDescriptor.Transient<IBackgroundJobWorker, RecordingBackgroundJobWorker>());

        // Two dedicated workers with different args types that both resolve to "shared-job-name".
        // AddDedicatedWorker's eager check compares by type (both pass); only the manager's backstop
        // (which resolves job names) can catch this.
        Configure<AbpBackgroundJobWorkerOptions>(options =>
        {
            options.AddDedicatedWorker<SharedNameJobAArgs>("lock-a");
            options.AddDedicatedWorker<SharedNameJobBArgs>("lock-b");
        });
    }
}
