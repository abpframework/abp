using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.CmsKit.Features;
using Volo.CmsKit.Public.Pages;

namespace Volo.CmsKit.Public.Web.Pages;

public class CmsKitHomePageRouteValueTransformer : DynamicRouteValueTransformer, ITransientDependency
{
    protected IFeatureChecker FeatureChecker { get; }

    protected IPagePublicAppService PagePublicAppService { get; }

    public CmsKitHomePageRouteValueTransformer(IFeatureChecker featureChecker, IPagePublicAppService pagePublicAppService)
    {
        FeatureChecker = featureChecker;
        PagePublicAppService = pagePublicAppService;
    }

    public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
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
