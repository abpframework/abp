using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpJwtBearerExtensions
{
    public static AuthenticationBuilder AddAbpJwtBearer(this AuthenticationBuilder builder)
        => builder.AddAbpJwtBearer(JwtBearerDefaults.AuthenticationScheme, _ => { });

    public static AuthenticationBuilder AddAbpJwtBearer(this AuthenticationBuilder builder, Action<JwtBearerOptions> configureOptions)
        => builder.AddAbpJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions);

    public static AuthenticationBuilder AddAbpJwtBearer(this AuthenticationBuilder builder, string authenticationScheme, Action<JwtBearerOptions> configureOptions)
        => builder.AddAbpJwtBearer(authenticationScheme, "Bearer", configureOptions);

    public static AuthenticationBuilder AddAbpJwtBearer(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<JwtBearerOptions> configureOptions)
    {
        builder.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            var jwtBearerOption = new JwtBearerOptions();
            configureOptions?.Invoke(jwtBearerOption);
            if (!jwtBearerOption.Authority.IsNullOrEmpty())
            {
                options.RemoteRefreshUrl = jwtBearerOption.Authority.RemovePostFix("/") + options.RemoteRefreshUrl;
            }
        });

        return builder.AddJwtBearer(authenticationScheme, displayName, options =>
        {
            configureOptions?.Invoke(options);
        });
    }
}
