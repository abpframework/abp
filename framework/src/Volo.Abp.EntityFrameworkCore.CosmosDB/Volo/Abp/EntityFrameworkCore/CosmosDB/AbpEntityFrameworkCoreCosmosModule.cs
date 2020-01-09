using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.Cosmos
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class AbpEntityFrameworkCoreCosmosModule : AbpModule
    {
    }
}
