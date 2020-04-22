using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.TextTemplating
{
    [DependsOn(
        typeof(AbpTextTemplatingModule),
        typeof(AbpTestBaseModule),
        typeof(AbpAutofacModule)
    )]
    public class AbpTextTemplatingTestModule : AbpModule
    {

    }
}
