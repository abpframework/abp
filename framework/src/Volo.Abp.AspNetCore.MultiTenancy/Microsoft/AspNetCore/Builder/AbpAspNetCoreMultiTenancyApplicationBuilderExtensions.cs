using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.MultiTenancy;

namespace Microsoft.AspNetCore.Builder;

public static class AbpAspNetCoreMultiTenancyApplicationBuilderExtensions
{
    private const string AuthenticationMiddlewareSetKey = "__AuthenticationMiddlewareSet";

    public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app)
    {
        var multiTenancyOptions = app.ApplicationServices.GetRequiredService<IOptions<AbpTenantResolveOptions>>();
        var hasCurrentUserTenantResolveContributor = multiTenancyOptions.Value.TenantResolvers.Any(r => r is CurrentUserTenantResolveContributor);
        if (hasCurrentUserTenantResolveContributor)
        {
            var authenticationMiddlewareSet = app.Properties.TryGetValue(AuthenticationMiddlewareSetKey, out var value) && value is true;
            if (!authenticationMiddlewareSet)
            {
                var logger = app.ApplicationServices.GetService<ILogger<MultiTenancyMiddleware>>();
                logger?.LogWarning(
                    "MultiTenancyMiddleware is being registered before the authentication middleware. " +
                    "This may lead to incorrect tenant resolution if the resolution depends on the authenticated user. " +
                    "Ensure app.UseAuthentication() is called before app.UseMultiTenancy()."
                );
            }
        }

        return app.UseMiddleware<MultiTenancyMiddleware>();
    }
}
