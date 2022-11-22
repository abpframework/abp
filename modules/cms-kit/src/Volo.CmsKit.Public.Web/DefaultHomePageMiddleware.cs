using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Public.Pages;

namespace Volo.CmsKit.Public.Web;

public class DefaultHomePageMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IPagePublicAppService _pagePublicAppService;

    public DefaultHomePageMiddleware(RequestDelegate next, IPagePublicAppService pagePublicAppService)
    {
        _next = next;
        _pagePublicAppService = pagePublicAppService;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var page = await _pagePublicAppService.FindDefaultHomePageAsync();
        if (page is not null && httpContext.Request.Path.Value == "/")
        {
            httpContext.Request.Path = $"{PageConsts.UrlPrefix}{page.Slug}";
        }

        await _next(httpContext);
    }
}