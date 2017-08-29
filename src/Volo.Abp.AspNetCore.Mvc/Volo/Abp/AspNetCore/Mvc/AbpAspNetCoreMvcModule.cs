using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.EmbeddedFiles;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc
{
    [DependsOn(
        typeof(AbpAspNetCoreModule)
    )]
    public class AbpAspNetCoreMvcModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAspNetCoreMvcModule>();

            //Configure Razor
            services.Insert(0,
                ServiceDescriptor.Singleton<IConfigureOptions<RazorViewEngineOptions>>(
                    new ConfigureOptions<RazorViewEngineOptions>(options =>
                        {
                            options.FileProviders.Add(
                                new EmbeddedResourceViewFileProvider(
                                    services.GetSingletonInstance<IObjectAccessor<IServiceProvider>>()
                                )
                            );
                        }
                    )
                )
            );
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            AddApplicationParts(context);
        }

        private static void AddApplicationParts(ApplicationInitializationContext context)
        {
            var partManager = context.ServiceProvider.GetService<ApplicationPartManager>();
            if (partManager == null)
            {
                return;
            }

            var moduleContainer = context.ServiceProvider.GetRequiredService<IModuleContainer>();

            foreach (var module in moduleContainer.Modules.Where(m => m.IsLoadedAsPlugIn))
            {
                partManager.ApplicationParts.Add(new AssemblyPart(module.Type.GetTypeInfo().Assembly));
            }
        }
    }
}
