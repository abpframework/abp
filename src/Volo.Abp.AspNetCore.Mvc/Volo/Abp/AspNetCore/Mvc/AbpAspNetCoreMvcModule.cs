using System;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.EmbeddedFiles;
using Volo.Abp.Modularity;
using Volo.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc
{
    [DependsOn(
        typeof(AbpAspNetCoreModule)
    )]
    public class AbpAspNetCoreMvcModule : IAbpModule
    {
        public void ConfigureServices(IServiceCollection services)
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
    }
}
