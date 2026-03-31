using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs;

[DependsOn(
    typeof(AbpBackgroundJobsModule),
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule)
)]
public class AbpBackgroundJobsTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<DynamicJobExecutionTracker>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        // Register handler via the singleton registry (through the transient manager).
        // The handler persists because IDynamicBackgroundJobHandlerRegistry is a singleton.
        var dynamicJobManager = context.ServiceProvider.GetRequiredService<IDynamicBackgroundJobManager>();
        var tracker = context.ServiceProvider.GetRequiredService<DynamicJobExecutionTracker>();

        dynamicJobManager.RegisterHandler("TestDynamicJob", (ctx, ct) =>
        {
            tracker.ExecutedJsonData.Add(ctx.JsonData);
            return System.Threading.Tasks.Task.CompletedTask;
        });
    }
}
