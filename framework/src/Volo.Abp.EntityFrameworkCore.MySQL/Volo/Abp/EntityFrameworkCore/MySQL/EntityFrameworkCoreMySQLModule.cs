using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.MySQL
{
    [DependsOn(
        typeof(EntityFrameworkCoreModule)
        )]
    public class EntityFrameworkCoreMySQLModule : AbpModule
    {

    }
}
