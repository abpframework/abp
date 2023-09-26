using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.CmsKit.Features;
using Volo.CmsKit.Public.Pages;

namespace Volo.CmsKit.Public.Web.Pages;

public class CmsKitPageRouteValueTransformer : DynamicRouteValueTransformer, ITransientDependency
{
    protected IFeatureChecker FeatureChecker { get; }
    protected IPagePublicAppService PagePublicAppService { get; }

    public CmsKitPageRouteValueTransformer(IFeatureChecker featureChecker, IPagePublicAppService pagePublicAppService)
    {
        FeatureChecker = featureChecker;
        PagePublicAppService = pagePublicAppService;
    }

    public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
    {
        if (values.TryGetValue("slug", out var slugParameter) && slugParameter is not null)
        {
            if (!await FeatureChecker.IsEnabledAsync(CmsKitFeatures.PageEnable))
            {
                return values;
            }

            var slug = slugParameter.ToString().TrimStart('/');
            var exist = await PagePublicAppService.DoesSlugExistAsync(slug);

            if (exist)
            {
                values["page"] = "/Public/CmsKit/Pages/Index";
            }
        }

        return values;
    }
}