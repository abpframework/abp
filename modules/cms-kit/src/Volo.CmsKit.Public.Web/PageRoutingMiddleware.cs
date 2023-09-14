using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.CmsKit.Features;
using Volo.CmsKit.Public.Pages;

namespace Volo.CmsKit.Public.Web;

public class PageRoutingMiddleware : IMiddleware, ITransientDependency
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next(context);

        if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
        {
            var featureChecker = context.RequestServices.GetRequiredService<IFeatureChecker>();
            if (!await featureChecker.IsEnabledAsync(CmsKitFeatures.PageEnable))
            {
                return;
            }

            var pagePublicAppService = context.RequestServices.GetRequiredService<IPagePublicAppService>();

            var page = await pagePublicAppService.FindBySlugAsync(
                context.Request.Path.ToString().TrimStart('/'));

            if (page is not null)
            {
                context.Request.Path = $"/pages/{page.Slug}";

                await next(context);
            }
        }
    }
}
