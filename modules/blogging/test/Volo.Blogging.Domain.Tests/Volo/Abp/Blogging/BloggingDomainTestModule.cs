using Volo.Abp.Modularity;

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