using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Security;

namespace Volo.Abp.AspNetCore.Components;

[DependsOn(
    typeof(AbpObjectMappingModule),
    typeof(AbpSecurityModule),
    typeof(AbpLocalizationModule)
    )]
public class AbpAspNetCoreComponentsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        DynamicProxyIgnoreTypes.Add<ComponentBase>();
        context.Services.AddConventionalRegistrar(new AbpWebAssemblyConventionalRegistrar());
    }
}
