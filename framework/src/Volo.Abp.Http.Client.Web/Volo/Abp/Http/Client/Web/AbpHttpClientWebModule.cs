using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Conventions;
using Volo.Abp.Http.Client.Web.Conventions;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http.Client.Web
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpHttpClientModule)
        )]
    public class AbpHttpClientWebModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Replace(ServiceDescriptor.Transient<IAbpServiceConvention, AbpHttpClientProxyServiceConvention>());
            context.Services.AddTransient<AbpHttpClientProxyServiceConvention>();

            var partManager = context.Services.GetSingletonInstance<ApplicationPartManager>();
            partManager.FeatureProviders.Add(new AbpHttpClientProxyControllerFeatureProvider());
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var partManager = context.ServiceProvider.GetRequiredService<ApplicationPartManager>();
            foreach (var moduleAssembly in context
                .ServiceProvider
                .GetRequiredService<IModuleContainer>()
                .Modules
                .Select(m => m.Type.Assembly)
                .Where(a => a.GetTypes().Any(AbpHttpClientProxyHelper.IsClientProxyService))
                .Distinct())
            {
                partManager.ApplicationParts.AddIfNotContains(moduleAssembly);
            }
        }
    }
}
