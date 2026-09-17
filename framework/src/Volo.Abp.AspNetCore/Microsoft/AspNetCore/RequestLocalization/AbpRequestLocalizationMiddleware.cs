using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Middleware;
using Volo.Abp.DependencyInjection;

namespace Microsoft.AspNetCore.RequestLocalization;

public class AbpRequestLocalizationMiddleware : AbpMiddlewareBase, ITransientDependency
{
    public const string HttpContextItemName = "__AbpSetCultureCookie";

    private readonly IAbpRequestLocalizationOptionsProvider _requestLocalizationOptionsProvider;
    private readonly ILoggerFactory _loggerFactory;

    public AbpRequestLocalizationMiddleware(
        IAbpRequestLocalizationOptionsProvider requestLocalizationOptionsProvider,
        ILoggerFactory loggerFactory)
    {
        _requestLocalizationOptionsProvider = requestLocalizationOptionsProvider;
        _loggerFactory = loggerFactory;
    }

    public async override Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var middleware = new RequestLocalizationMiddleware(
            next,
            new OptionsWrapper<RequestLocalizationOptions>(
                await _requestLocalizationOptionsProvider.GetLocalizationOptionsAsync()
            ),
            _loggerFactory
        );

        context.Response.OnStarting(() =>
        {
            if (context.Items[HttpContextItemName] == null)
            {
                var requestCultureFeature = context.Features.Get<IRequestCultureFeature>();
                if (requestCultureFeature?.Provider is QueryStringRequestCultureProvider
                    or RouteDataRequestCultureProvider)
                {
                    AbpRequestCultureCookieHelper.SetCultureCookie(
                        context,
                        requestCultureFeature.RequestCulture
                    );
                }

                // Only manage HasRouteCulture cookie for Blazor component page requests.
                // This cookie is used by AbpCultureMenuItemUrlProvider to determine if the
                // initial SSR page had a culture prefix, since the Blazor interactive circuit
                // (/_blazor) does not carry the original route values.
                // Note: ComponentTypeMetadata is an internal ASP.NET Core type
                // (Microsoft.AspNetCore.Components.Endpoints.ComponentTypeMetadata).
                // We match by full type name to avoid false positives from other assemblies.
                var endpoint = context.GetEndpoint();
                if (endpoint?.Metadata.Any(m => m.GetType().FullName == "Microsoft.AspNetCore.Components.Endpoints.ComponentTypeMetadata") == true)
                {
                    AbpRequestCultureCookieHelper.SetHasRouteCultureCookie(
                        context, requestCultureFeature?.Provider is RouteDataRequestCultureProvider);
                }
            }

            return Task.CompletedTask;
        });

        await middleware.Invoke(context);
    }
}
