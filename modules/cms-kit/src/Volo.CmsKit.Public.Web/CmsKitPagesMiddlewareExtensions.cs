using Microsoft.AspNetCore.Builder;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.Web;

public static class CmsKitPagesMiddlewareExtensions
{
    public static IApplicationBuilder UseHomePageDefaultMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DefaultHomePageMiddleware>();
    }

    public static IApplicationBuilder UseCmsKitPagesMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<PageRoutingMiddleware>();
    }
}
