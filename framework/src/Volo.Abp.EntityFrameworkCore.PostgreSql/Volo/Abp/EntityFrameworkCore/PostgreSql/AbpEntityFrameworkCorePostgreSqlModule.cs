using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.PostgreSql
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class AbpEntityFrameworkCorePostgreSqlModule : AbpModule
    {

    }
}
