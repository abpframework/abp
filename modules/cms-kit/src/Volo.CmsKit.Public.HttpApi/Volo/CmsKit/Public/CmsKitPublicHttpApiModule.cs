using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Modularity;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Public.RobotsTxts;

namespace Volo.CmsKit.Public;

[DependsOn(
    typeof(CmsKitPublicApplicationContractsModule),
    typeof(CmsKitCommonHttpApiModule))]
public class CmsKitPublicHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CmsKitPublicHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        if (GlobalFeatureManager.Instance.IsEnabled<RobotsTxtFeature>())
        {
            Configure<AbpEndpointRouterOptions>(options =>
            {
                options.EndpointConfigureActions.Add(endpointContext =>
                {
                    var robotsTxtOptions = endpointContext.ScopeServiceProvider
                        .GetRequiredService<IOptions<CmsKitRobotsTxtOptions>>().Value;

                    if (robotsTxtOptions.IsEndpointEnabled)
                    {
                        endpointContext.Endpoints.MapCmsRobotsTxt();
                    }
                });
            });
        }
    }
}
