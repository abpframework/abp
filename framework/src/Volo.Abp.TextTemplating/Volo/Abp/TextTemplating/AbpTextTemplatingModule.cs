using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating.Scriban;

namespace Volo.Abp.TextTemplating
{
    [DependsOn(
        typeof(AbpTextTemplatingScribanModule)
    )]
    public class AbpTextTemplatingModule : AbpModule
    {

    }
}
