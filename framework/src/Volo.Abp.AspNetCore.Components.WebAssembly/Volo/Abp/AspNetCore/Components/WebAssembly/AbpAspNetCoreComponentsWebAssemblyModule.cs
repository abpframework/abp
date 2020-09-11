using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.UI;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcClientCommonModule),
        typeof(AbpUiModule)
        )]
    public class AbpAspNetCoreComponentsWebAssemblyModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpHttpClientBuilderOptions>(options =>
            {
                options.ProxyClientBuildActions.Add((_, builder) =>
                {
                    builder.AddHttpMessageHandler<AbpBlazorClientHttpMessageHandler>();
                });
            });
        }
    }
}
