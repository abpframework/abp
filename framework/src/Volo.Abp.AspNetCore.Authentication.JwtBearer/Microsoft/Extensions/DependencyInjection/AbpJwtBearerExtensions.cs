using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Authentication;
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

            options.Events ??= new JwtBearerEvents();
            var previousOnChallenge = options.Events.OnChallenge;
            options.Events.OnChallenge = async eventContext =>
            {
                await previousOnChallenge(eventContext);

                if (eventContext.Handled ||
                    !string.IsNullOrEmpty(eventContext.Error) ||
                    !string.IsNullOrEmpty(eventContext.ErrorDescription) ||
                    !string.IsNullOrEmpty(eventContext.ErrorUri))
                {
                    return;
                }

                var tokenUnauthorizedErrorInfo = eventContext.HttpContext.RequestServices.GetRequiredService<AbpAspNetCoreTokenUnauthorizedErrorInfo>();
                if (string.IsNullOrEmpty(tokenUnauthorizedErrorInfo.Error) &&
                    string.IsNullOrEmpty(tokenUnauthorizedErrorInfo.ErrorDescription) &&
                    string.IsNullOrEmpty(tokenUnauthorizedErrorInfo.ErrorUri))
                {
                    return;
                }

                eventContext.Error = tokenUnauthorizedErrorInfo.Error;
                eventContext.ErrorDescription = tokenUnauthorizedErrorInfo.ErrorDescription;
                eventContext.ErrorUri = tokenUnauthorizedErrorInfo.ErrorUri;
            };
        });
    }
}
