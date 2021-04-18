using Volo.Abp.Modularity;

namespace Volo.Abp.TextTemplating.Razor
{
    [DependsOn(
        typeof(AbpTextTemplatingAbstractionsModule)
    )]
    public class AbpTextTemplatingRazorModule : AbpModule
    {

    }
}
