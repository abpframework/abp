using Volo.Abp.Modularity;
using Volo.Abp.BlazoriseUI;

namespace Volo.Abp.Identity.Blazor
{
    [DependsOn(
        typeof(AbpIdentityHttpApiClientModule),
        typeof(AbpBlazoriseUIModule)
        )]
    public class AbpIdentityBlazorModule : AbpModule
    {
    }
}
