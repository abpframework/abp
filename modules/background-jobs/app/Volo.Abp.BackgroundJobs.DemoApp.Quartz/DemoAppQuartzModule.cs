using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.DemoApp.Shared;
using Volo.Abp.BackgroundJobs.Quartz;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs.DemoApp.Quartz
{
    [DependsOn(
        typeof(DemoAppSharedModule),
        typeof(AbpAutofacModule),
        typeof(AbpBackgroundJobsQuartzModule)
    )]
    public class DemoAppQuartzModule : AbpModule
    {

    }
}
