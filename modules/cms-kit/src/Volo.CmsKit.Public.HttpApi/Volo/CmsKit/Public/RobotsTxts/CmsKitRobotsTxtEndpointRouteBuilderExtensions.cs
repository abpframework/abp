using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.RobotsTxts;

public static class CmsKitRobotsTxtEndpointRouteBuilderExtensions
{
    public const string RobotsTxtRoute = "/robots.txt";

    /// <summary>
    /// Maps the "/robots.txt" endpoint that serves the robots.txt content of the current tenant.
    /// It has the highest matching order, so it takes priority over a static wwwroot/robots.txt file,
    /// which is used as the default content.
    /// <see cref="CmsKitPublicHttpApiModule"/> calls this method automatically
    /// unless <see cref="CmsKitRobotsTxtOptions.IsEndpointEnabled"/> is false.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="order">The matching order of the endpoint. Lower is prior.</param>
    public static IEndpointRouteBuilder MapCmsRobotsTxt(this IEndpointRouteBuilder builder, int order = int.MinValue)
    {
        if (!GlobalFeatureManager.Instance.IsEnabled<RobotsTxtFeature>())
        {
            return builder;
        }

        builder
            .MapGet(RobotsTxtRoute, HandleAsync)
            .WithOrder(order)
            .AllowAnonymous()
            .ExcludeFromDescription();

        return builder;
    }

    private static async Task HandleAsync(HttpContext httpContext)
    {
        var result = await GetResultAsync(httpContext);
        await result.ExecuteAsync(httpContext);
    }

    private static async Task<IResult> GetResultAsync(HttpContext httpContext)
    {
        var defaultContentProvider = httpContext.RequestServices.GetRequiredService<IRobotsTxtDefaultContentProvider>();
        var featureChecker = httpContext.RequestServices.GetRequiredService<IFeatureChecker>();

        string content;
        if (await featureChecker.IsEnabledAsync(CmsKitFeatures.RobotsTxtEnable))
        {
            var robotsTxtPublicAppService = httpContext.RequestServices.GetRequiredService<IRobotsTxtPublicAppService>();
            content = (await robotsTxtPublicAppService.GetAsync()).Content ?? await defaultContentProvider.GetAsync();
        }
        else
        {
            // The feature is disabled for the current tenant: behave as if this endpoint doesn't exist.
            content = await defaultContentProvider.GetStaticFileContentOrNullAsync();
            if (content == null)
            {
                return Results.NotFound();
            }
        }

        return Results.Text(content, "text/plain", Encoding.UTF8);
    }
}
