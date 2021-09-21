using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Conventions;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle.Conventions;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Swashbuckle
{
    [DependsOn(
        typeof(AbpVirtualFileSystemModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpHttpClientModule))]
    public class AbpSwashbuckleModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpSwashbuckleModule>();
            });

            var swaggerConventionOptions = context.Services.ExecutePreConfiguredActions<AbpSwaggerClientProxyOptions>();
            if (swaggerConventionOptions.IsEnabled)
            {
                context.Services.Replace(ServiceDescriptor.Transient<IAbpServiceConvention, AbpSwaggerServiceConvention>());
                context.Services.AddTransient<AbpSwaggerServiceConvention>();

                var partManager = context.Services.GetSingletonInstance<ApplicationPartManager>();
                partManager.FeatureProviders.Add(new AbpSwaggerClientProxyControllerFeatureProvider());
            }
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var swaggerConventionOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpSwaggerClientProxyOptions>>().Value;
            if (swaggerConventionOptions.IsEnabled)
            {
                var partManager = context.ServiceProvider.GetRequiredService<ApplicationPartManager>();
                foreach (var moduleAssembly in context
                    .ServiceProvider
                    .GetRequiredService<IModuleContainer>()
                    .Modules
                    .Select(m => m.Type.Assembly)
                    .Where(a => a.GetTypes().Any(AbpSwaggerClientProxyHelper.IsClientProxyService))
                    .Distinct())
                {
                    partManager.ApplicationParts.AddIfNotContains(moduleAssembly);
                }
            }
        }
    }
}
