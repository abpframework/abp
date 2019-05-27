using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.PostgreSql
{
    [DependsOn(
        typeof(EntityFrameworkCoreModule)
        )]
    public class EntityFrameworkCorePostgreSqlModule : AbpModule
    {

    }
}
