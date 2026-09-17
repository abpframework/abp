using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Middleware;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Security.Claims;

public class AbpDynamicClaimsMiddleware : AbpMiddlewareBase, ITransientDependency
{
    public async override Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            if (context.RequestServices.GetRequiredService<IOptions<AbpClaimsPrincipalFactoryOptions>>().Value.IsDynamicClaimsEnabled)
            {
                var authenticateResultFeature = context.Features.Get<IAuthenticateResultFeature>();
                var authenticationType = authenticateResultFeature?.AuthenticateResult?.Ticket?.AuthenticationScheme ?? context.User.Identity.AuthenticationType;

                if (authenticateResultFeature != null && !authenticationType.IsNullOrWhiteSpace())
                {
                    var abpClaimsPrincipalFactory = context.RequestServices.GetRequiredService<IAbpClaimsPrincipalFactory>();
                    var user = await abpClaimsPrincipalFactory.CreateDynamicAsync(context.User);

                    authenticateResultFeature.AuthenticateResult = AuthenticateResult.Success(new AuthenticationTicket(
                        user,
                        authenticateResultFeature?.AuthenticateResult?.Properties,
                        authenticationType));
                }

                if (context.User.Identity?.IsAuthenticated == false)
                {
                    var authenticationSchemeProvider = context.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();
                    if (!authenticationType.IsNullOrWhiteSpace())
                    {
                        var authenticationScheme = await authenticationSchemeProvider.GetSchemeAsync(authenticationType);
                        if (authenticationScheme != null && typeof(IAuthenticationSignOutHandler).IsAssignableFrom(authenticationScheme.HandlerType))
                        {
                            await context.SignOutAsync(authenticationScheme.Name);
                        }
                    }
                }
            }
        }

        await next(context);
    }
}
