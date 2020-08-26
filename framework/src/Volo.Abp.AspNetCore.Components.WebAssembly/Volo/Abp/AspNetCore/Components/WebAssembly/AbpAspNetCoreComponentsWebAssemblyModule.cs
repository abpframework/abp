using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcClientCommonModule)
        )]
    public class AbpAspNetCoreComponentsWebAssemblyModule : AbpModule
    {

    }
}
