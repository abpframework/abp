using Volo.Docs.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsEntityFrameworkCoreTestModule),
        typeof(DocsTestBaseModule)
        )]
    public class DocsDomainTestModule : AbpModule
    {
        
    }
}
