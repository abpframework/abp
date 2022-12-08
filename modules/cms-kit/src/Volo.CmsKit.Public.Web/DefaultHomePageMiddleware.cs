using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Public.Pages;

namespace Volo.CmsKit.Public.Web;

public class DefaultHomePageMiddleware : IMiddleware, ITransientDependency
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var _pagePublicAppService = context.RequestServices.GetRequiredService<IPagePublicAppService>();
        var page = await _pagePublicAppService.FindDefaultHomePageAsync();
        if (page is not null && context.Request.Path.Value == "/")
        {
            context.Request.Path = $"{PageConsts.UrlPrefix}{page.Slug}";
        }
        
        await next(context);
    }
}