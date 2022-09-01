using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pages.Abp.MultiTenancy.ClientProxies;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ClientProxies;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Features;
using Volo.Abp.Http.Client;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.Client;

[DependsOn(
    typeof(AbpHttpClientModule),
    typeof(AbpAspNetCoreMvcContractsModule),
    typeof(AbpCachingModule),
    typeof(AbpLocalizationModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpFeaturesModule),
    typeof(AbpVirtualFileSystemModule)
)]
public class AbpAspNetCoreMvcClientCommonModule : AbpModule
{
    public const string RemoteServiceName = "AbpMvcClient";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(typeof(AbpAspNetCoreMvcContractsModule).Assembly, RemoteServiceName);

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAspNetCoreMvcClientCommonModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.GlobalContributors.Add<RemoteLocalizationContributor>();
        });

        context.Services.AddTransient<AbpApplicationConfigurationClientProxy>();
        context.Services.AddTransient<AbpTenantClientProxy>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpAspNetCoreMvcClientCommonOptions>>().Value;
        if (options.GetApplicationConfigurationOnModuleInitialization)
        {
            AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
        }
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpAspNetCoreMvcClientCommonOptions>>().Value;
        if (options.GetApplicationConfigurationOnModuleInitialization)
        {
            await context.ServiceProvider.GetRequiredService<ICachedApplicationConfigurationClient>().GetAsync();
        }
    }
}
