using Microsoft.AspNetCore.Builder;

namespace Volo.CmsKit.Public.Web;

public static class HomePageDefaultMiddlewareExtensions
{
    public static IApplicationBuilder UseHomePageDefaultMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DefaultHomePageMiddleware>();
    }
}
