using Volo.Abp.Modularity;

namespace Volo.Abp.Identity.Blazor
{
    [DependsOn(
        typeof(AbpIdentityHttpApiClientModule)
        )]
    public class AbpIdentityBlazorModule : AbpModule
    {

    }
}
