using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity.Blazor.Server
{
    [DependsOn(
        typeof(AbpIdentityBlazorModule),
        typeof(AbpAspNetCoreComponentsServerThemingModule)
    )]
    public class AbpIdentityBlazorServerModule : AbpModule
    {
    }
}
