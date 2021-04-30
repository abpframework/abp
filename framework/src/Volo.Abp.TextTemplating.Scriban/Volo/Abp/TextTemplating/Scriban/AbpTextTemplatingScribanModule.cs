using Volo.Abp.Modularity;

namespace Volo.Abp.TextTemplating.Scriban
{
    [DependsOn(
        typeof(AbpTextTemplatingAbstractionsModule)
    )]
    public class AbpTextTemplatingScribanModule : AbpModule
    {

    }
}
