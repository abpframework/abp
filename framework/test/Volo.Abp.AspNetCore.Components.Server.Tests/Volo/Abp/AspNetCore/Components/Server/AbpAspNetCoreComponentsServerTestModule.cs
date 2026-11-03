using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.Server;

[DependsOn(typeof(AbpAspNetCoreComponentsServerModule))]
public class AbpAspNetCoreComponentsServerTestModule : AbpModule
{
}
