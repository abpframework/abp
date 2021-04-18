using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating.Razor;
using Volo.Abp.TextTemplating.Scriban;

namespace Volo.Abp.TextTemplating
{
    [DependsOn(
        typeof(AbpTextTemplatingAbstractionsModule),
        typeof(AbpTextTemplatingScribanModule),
        typeof(AbpTextTemplatingRazorModule)
    )]
    public class AbpTextTemplatingModule : AbpModule
    {

    }
}
