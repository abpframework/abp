using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.Web;

public static class CmsKitPagesMiddlewareExtensions
{
    private const string UseCmsKitHomePageMiddlewareKey = "UseCmsKitHomePageMiddleware";

    [Obsolete("Use UseCmsKitPagesMiddleware instead.")]
    public static IApplicationBuilder UseCmsKitHomePageMiddleware(this IApplicationBuilder builder)
    {
        if (!GlobalFeatureManager.Instance.IsEnabled<PagesFeature>())
        {
            return builder;
        }

        builder.Properties[UseCmsKitHomePageMiddlewareKey] = true;
        return builder.UseMiddleware<DefaultHomePageMiddleware>();
    }

    public static IApplicationBuilder UseCmsKitPagesMiddleware(this IApplicationBuilder builder)
    {
        if (!GlobalFeatureManager.Instance.IsEnabled<PagesFeature>())
        {
            return builder;
        }

        if (!builder.Properties.ContainsKey(UseCmsKitHomePageMiddlewareKey))
        {
            builder.Properties[UseCmsKitHomePageMiddlewareKey] = true;
            builder.UseMiddleware<DefaultHomePageMiddleware>();
        }

        return builder.UseMiddleware<PageRoutingMiddleware>();
    }
}
