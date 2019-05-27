using Volo.Abp.Modularity;

namespace Volo.Abp.Cli
{
    [DependsOn(
        typeof(TestBaseModule),
        typeof(CliCoreModule)
        )]
    public class AbpCliTestModule : AbpModule
    {

    }
}
