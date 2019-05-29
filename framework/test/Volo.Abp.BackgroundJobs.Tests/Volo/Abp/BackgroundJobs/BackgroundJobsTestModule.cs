using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs
{
    [DependsOn(
        typeof(BackgroundJobsModule),
        typeof(AutofacModule),
        typeof(TestBaseModule)
    )]
    public class BackgroundJobsTestModule : AbpModule
    {

    }
}