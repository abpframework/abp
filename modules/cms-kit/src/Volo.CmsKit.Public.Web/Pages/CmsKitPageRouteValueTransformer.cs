using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Volo.Abp.Auditing;
using Volo.Abp.Caching;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Volo.CmsKit.Features;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Public.Pages;
using Volo.CmsKit.Public.Web.Pages.Public;

namespace Volo.CmsKit.Public.Web.Pages;

public class CmsKitPageRouteValueTransformer : CmsKitDynamicRouteValueTransformerBase
{
    protected IFeatureChecker FeatureChecker { get; }
    protected IPagePublicAppService PagePublicAppService { get; }
    protected IDistributedCache<PageCacheItem> PageCache { get; }
    protected IAuditingHelper AuditingHelper { get; }

    public CmsKitPageRouteValueTransformer(
        ICurrentTenant currentTenant,
        ITenantConfigurationProvider tenantConfigurationProvider,
        IFeatureChecker featureChecker,
        IPagePublicAppService pagePublicAppService,
        IDistributedCache<PageCacheItem> pageCache,
        IAuditingHelper auditingHelper)
        : base(currentTenant, tenantConfigurationProvider)
    {
        FeatureChecker = featureChecker;
        PagePublicAppService = pagePublicAppService;
        PageCache = pageCache;
        AuditingHelper = auditingHelper;
    }

    protected async override ValueTask<RouteValueDictionary> DoTransformAsync(HttpContext httpContext, RouteValueDictionary values)
    {
        if (values.TryGetValue("slug", out var slugParameter) && slugParameter is not null)
        {
            if (!await FeatureChecker.IsEnabledAsync(CmsKitFeatures.PageEnable))
            {
                return values;
            }

            var slug = slugParameter.ToString().TrimStart('/');

            var exist = await PageCache.GetAsync(PageCacheItem.GetKey(slug)) != null;
            if (!exist)
            {
                using (AuditingHelper.DisableAuditing())
                {
                    exist = await PagePublicAppService.DoesSlugExistAsync(slug);
                }
            }

            if (exist)
            {
                values["page"] = "/Public/CmsKit/Pages/Index";
            }
        }

        return values;
    }
}
