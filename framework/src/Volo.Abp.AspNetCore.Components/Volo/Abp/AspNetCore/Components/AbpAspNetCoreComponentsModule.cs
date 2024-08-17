using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Security;
using Volo.Abp.Timing;

namespace Volo.Abp.AspNetCore.Components;

[DependsOn(
    typeof(AbpObjectMappingModule),
    typeof(AbpSecurityModule),
    typeof(AbpTimingModule),
    typeof(AbpMultiTenancyAbstractionsModule)
    )]
public class AbpAspNetCoreComponentsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        DynamicProxyIgnoreTypes.Add<ComponentBase>();
        context.Services.AddConventionalRegistrar(new AbpWebAssemblyConventionalRegistrar());
    }
}
