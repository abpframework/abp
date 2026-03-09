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

        Configure<AbpBackgroundJobOptions>(options =>
        {
            options.AddDynamicJob("TestDynamicJob", context =>
            {
                var tracker = context.ServiceProvider.GetRequiredService<DynamicJobExecutionTracker>();
                tracker.ExecutedArgs.Add(context.Args);
                return System.Threading.Tasks.Task.CompletedTask;
            });
        });
    }
}
