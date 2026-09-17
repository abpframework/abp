using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Volo.CmsKit.Features;
using Volo.CmsKit.Public.Pages;
using Volo.CmsKit.Public.Web.Pages.Public;

namespace Volo.CmsKit.Public.Web.Pages;

public class CmsKitHomePageRouteValueTransformer : CmsKitDynamicRouteValueTransformerBase
{
    protected IFeatureChecker FeatureChecker { get; }

    protected IPagePublicAppService PagePublicAppService { get; }

    public CmsKitHomePageRouteValueTransformer(
        ICurrentTenant currentTenant,
        ITenantConfigurationProvider tenantConfigurationProvider,
        IFeatureChecker featureChecker,
        IPagePublicAppService pagePublicAppService)
        : base(currentTenant, tenantConfigurationProvider)
    {
        FeatureChecker = featureChecker;
        PagePublicAppService = pagePublicAppService;
    }

    protected async override ValueTask<RouteValueDictionary> DoTransformAsync(HttpContext httpContext, RouteValueDictionary values)
    {
        if (await FeatureChecker.IsEnabledAsync(CmsKitFeatures.PageEnable))
        {
            var page = await PagePublicAppService.FindDefaultHomePageAsync();
            if (page is not null)
            {
                values = new RouteValueDictionary();

                values["page"] = "/Public/CmsKit/Pages/Index";
                values["slug"] = page.Slug;
            }
        }

        return values;
    }
}
