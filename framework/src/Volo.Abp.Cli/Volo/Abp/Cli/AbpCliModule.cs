using Volo.Abp.Modularity;

namespace Volo.Abp.Cli
{
    [DependsOn(
        typeof(AbpCliCoreModule)
    )]
    public class AbpCliModule : AbpModule
    {

    }
}