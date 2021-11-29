using Volo.Abp.Cli;
using Volo.Abp.Modularity;

namespace Volo.Abp.Studio
{
    [DependsOn(
        typeof(AbpCliCoreModule)
    )]
    public class AbpStudioDomainSharedModule : AbpModule
    {

    }
}
