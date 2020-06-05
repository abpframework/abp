using Volo.Abp.Modularity;

namespace Volo.Abp.Cli
{
    [DependsOn(
        typeof(AbpTestBaseModule),
        typeof(AbpCliCoreModule)
        )]
    public class AbpCliTestModule : AbpModule
    {

    }
}
