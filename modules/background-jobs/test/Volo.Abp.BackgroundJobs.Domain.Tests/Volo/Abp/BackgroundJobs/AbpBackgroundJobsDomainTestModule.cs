using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs;

[DependsOn(
    typeof(AbpBackgroundJobsEntityFrameworkCoreTestModule)
    )]
public class AbpBackgroundJobsDomainTestModule : AbpModule
{

}
