using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.Server
{
    [DependsOn(
        typeof(AbpAspNetCoreModule)
        )]
    public class AbpAspNetCoreComponentsServerModule : AbpModule
    {
    }
}