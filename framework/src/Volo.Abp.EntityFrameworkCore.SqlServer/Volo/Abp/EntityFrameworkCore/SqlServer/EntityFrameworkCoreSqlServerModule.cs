using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.SqlServer
{
    [DependsOn(
        typeof(EntityFrameworkCoreModule)
        )]
    public class EntityFrameworkCoreSqlServerModule : AbpModule
    {

    }
}
