using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs
{
    [DependsOn(
        typeof(BackgroundJobsEntityFrameworkCoreTestModule)
        )]
    public class BackgroundJobsDomainTestModule : AbpModule
    {
        
    }
}
