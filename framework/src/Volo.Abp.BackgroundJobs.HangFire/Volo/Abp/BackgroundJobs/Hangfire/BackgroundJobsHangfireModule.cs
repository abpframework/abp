using Volo.Abp.Hangfire;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs.Hangfire
{
    [DependsOn(
        typeof(BackgroundJobsAbstractionsModule),
        typeof(HangfireModule)
        )]
    public class BackgroundJobsHangfireModule : AbpModule
    {

    }
}
