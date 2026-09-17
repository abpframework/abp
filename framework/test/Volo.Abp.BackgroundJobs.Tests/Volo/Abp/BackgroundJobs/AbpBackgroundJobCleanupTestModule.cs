using System;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs;

[DependsOn(
    typeof(AbpBackgroundJobsModule),
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule)
)]
public class AbpBackgroundJobCleanupTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // IsJobExecutionEnabled is true by default, which the cleanup worker requires.
        Configure<AbpBackgroundJobWorkerOptions>(options =>
        {
            options.StoreSuccessfulJobs = true;
            options.SuccessfulJobRetentionTime = TimeSpan.FromDays(1);
        });
    }
}
