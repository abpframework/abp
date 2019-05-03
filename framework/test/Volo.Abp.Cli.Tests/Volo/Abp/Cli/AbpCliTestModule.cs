using Volo.Abp.Modularity;

namespace Volo.Abp.Cli
{
    [DependsOn(
        typeof(AbpTestBaseModule),
        typeof(AbpCliModule)
        )]
    public class AbpCliTestModule : AbpModule
    {

    }
}
