using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.SqlServer
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class AbpEntityFrameworkCoreSqlServerModule : AbpModule
    {

    }
}
