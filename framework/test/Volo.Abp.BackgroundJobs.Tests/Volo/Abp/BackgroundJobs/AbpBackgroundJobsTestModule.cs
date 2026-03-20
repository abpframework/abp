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
        context.Services.AddSingleton<AnonymousJobExecutionTracker>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var dynamicJobManager = context.ServiceProvider.GetRequiredService<IDynamicBackgroundJobManager>();
        var tracker = context.ServiceProvider.GetRequiredService<AnonymousJobExecutionTracker>();

        dynamicJobManager.RegisterHandler("TestAnonymousJob", (ctx, ct) =>
        {
            tracker.ExecutedJsonData.Add(ctx.JsonData);
            return System.Threading.Tasks.Task.CompletedTask;
        });
    }
}
