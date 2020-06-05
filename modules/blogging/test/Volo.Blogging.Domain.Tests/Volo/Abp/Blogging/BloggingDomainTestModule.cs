using Volo.Abp.Modularity;
using Volo.Blogging.EntityFrameworkCore;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingEntityFrameworkCoreTestModule),
        typeof(BloggingTestBaseModule)
    )]
    public class BloggingDomainTestModule : AbpModule
    {
    }
}