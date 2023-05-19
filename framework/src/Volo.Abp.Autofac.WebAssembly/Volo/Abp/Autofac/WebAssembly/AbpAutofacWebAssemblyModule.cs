using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.Modularity;

namespace Volo.Abp.Autofac.WebAssembly;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyModule)
    )]
public class AbpAutofacWebAssemblyModule : AbpModule
{

}
