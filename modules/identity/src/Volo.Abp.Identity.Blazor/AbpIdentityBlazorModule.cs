using Volo.Abp.Modularity;
using Volo.Abp.BlazoriseLib;

namespace Volo.Abp.Identity.Blazor
{
    [DependsOn(
        typeof(AbpIdentityHttpApiClientModule),
        typeof(AbpBlazoriseModule)
        )]
    public class AbpIdentityBlazorModule : AbpModule
    {
    }
}
