using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Volo.Abp.AspNetCore.MultiTenancy;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpOpenIdConnectExtensions
    {
        public static AuthenticationBuilder AddAbpOpenIdConnect(this AuthenticationBuilder builder)
            => builder.AddAbpOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddAbpOpenIdConnect(this AuthenticationBuilder builder, Action<OpenIdConnectOptions> configureOptions)
            => builder.AddAbpOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddAbpOpenIdConnect(this AuthenticationBuilder builder, string authenticationScheme, Action<OpenIdConnectOptions> configureOptions)
            => builder.AddAbpOpenIdConnect(authenticationScheme, OpenIdConnectDefaults.DisplayName, configureOptions);

        public static AuthenticationBuilder AddAbpOpenIdConnect(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<OpenIdConnectOptions> configureOptions)
        {
            return builder.AddOpenIdConnect(authenticationScheme, displayName, options =>
            {
                options.ClaimActions.MapAbpClaimTypes();

                configureOptions?.Invoke(options);

                options.Events ??= new OpenIdConnectEvents();
                var authorizationCodeReceived = options.Events.OnAuthorizationCodeReceived ?? (_ => Task.CompletedTask);

                options.Events.OnAuthorizationCodeReceived = receivedContext =>
                {
                    SetAbpTenantId(receivedContext);
                    return authorizationCodeReceived.Invoke(receivedContext);
                };

                options.Events.OnRemoteFailure = remoteFailureContext =>
                {
                    if (remoteFailureContext.Failure is OpenIdConnectProtocolException &&
                        remoteFailureContext.Failure.Message.Contains("access_denied"))
                    {
                        remoteFailureContext.HandleResponse();
                        remoteFailureContext.Response.Redirect($"{remoteFailureContext.Request.PathBase}/");
                    }
                    return Task.CompletedTask;
                };
            });
        }

        private static void SetAbpTenantId(AuthorizationCodeReceivedContext receivedContext)
        {
            var tenantKey = receivedContext.HttpContext.RequestServices
                .GetRequiredService<IOptions<AbpAspNetCoreMultiTenancyOptions>>().Value.TenantKey;

            if (receivedContext.Request.Cookies.ContainsKey(tenantKey))
            {
                receivedContext.TokenEndpointRequest.SetParameter(tenantKey,
                    receivedContext.Request.Cookies[tenantKey]);
            }
        }
    }
}
