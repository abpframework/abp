using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.Http.Client;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.Client
{
    [DependsOn(
        typeof(HttpClientModule),
        typeof(AspNetCoreMvcContractsModule),
        typeof(CachingModule),
        typeof(LocalizationModule)
        )]
    public class AspNetCoreMvcClientModule : AbpModule
    {
        public const string RemoteServiceName = "AbpMvcClient";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AspNetCoreMvcContractsModule).Assembly,
                RemoteServiceName,
                asDefaultServices: false
            );

            Configure<AbpLocalizationOptions>(options =>
            {
                options.GlobalContributors.Add<RemoteLocalizationContributor>();
            });
        }
    }
}
