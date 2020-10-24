using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components
{
    public class AbpAspNetCoreComponentsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            DynamicProxyIgnoreTypes.Add<ComponentBase>();
            context.Services.AddConventionalRegistrar(new AbpWebAssemblyConventionalRegistrar());
        }
    }
}
