using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Extensions.DependencyInjection;

public static class AbpAspNetCoreServiceCollectionExtensions
{
    public static IServiceCollection ForwardIdentityAuthenticationForBearer(this IServiceCollection services, string jwtBearerScheme = "Bearer")
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.ForwardDefaultSelector = ctx =>
            {
                string authorization = ctx.Request.Headers.Authorization;
                if (!authorization.IsNullOrWhiteSpace() && authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    return jwtBearerScheme;
                }

                return null;
            };
        });

        return services;
    }
}
