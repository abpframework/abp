using Volo.Docs.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsEntityFrameworkCoreTestModule)
        )]
    public class DocsDomainTestModule : AbpModule
    {
        
    }
}
