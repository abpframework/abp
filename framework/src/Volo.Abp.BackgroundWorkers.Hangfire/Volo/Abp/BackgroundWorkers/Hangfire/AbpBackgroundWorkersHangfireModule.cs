using Volo.Abp.Hangfire;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundWorkers.Hangfire
{
    [DependsOn(
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpHangfireModule)
    )]
    public class AbpBackgroundWorkersHangfireModule : AbpModule
    {
    }
}