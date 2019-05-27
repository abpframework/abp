using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.Sqlite
{
    [DependsOn(
        typeof(EntityFrameworkCoreModule)
    )]
    public class EntityFrameworkCoreSqliteModule : AbpModule
    {

    }
}
