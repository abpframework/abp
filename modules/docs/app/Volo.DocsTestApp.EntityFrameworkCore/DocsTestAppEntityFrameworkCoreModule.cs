using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;
using Volo.Docs.EntityFrameworkCore;

namespace Volo.DocsTestApp.EntityFrameworkCore
{
    [DependsOn(
        typeof(DocsEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule))]
    public class DocsTestAppEntityFrameworkCoreModule : AbpModule
    {
        
    }
}
