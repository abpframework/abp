using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.ExceptionHandling
{
    public class DefaultAbpAuthorizationExceptionHandler : IAbpAuthorizationExceptionHandler, ITransientDependency
    {
        public virtual async Task<bool> HandleAsync(AbpAuthorizationException exception, HttpContext httpContext)
        {
            var handlerOptions = httpContext.RequestServices.GetRequiredService<IOptions<AbpAuthorizationExceptionHandlerOptions>>().Value;
            if (handlerOptions.UseAuthenticationScheme)
            {
                var isAuthenticated = httpContext.User.Identity?.IsAuthenticated ?? false;
                var authenticationSchemeProvider = httpContext.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();
                var scheme = !handlerOptions.AuthenticationScheme.IsNullOrWhiteSpace()
                    ? await authenticationSchemeProvider.GetSchemeAsync(handlerOptions.AuthenticationScheme)
                    : isAuthenticated
                        ? await authenticationSchemeProvider.GetDefaultForbidSchemeAsync()
                        : await authenticationSchemeProvider.GetDefaultChallengeSchemeAsync();

                if (scheme != null)
                {
                    var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
                    var handler = await handlers.GetHandlerAsync(httpContext, scheme.Name);
                    if (handler != null)
                    {
                        if (isAuthenticated)
                        {
                            await handler.ForbidAsync(null);
                        }
                        else
                        {
                            await handler.ChallengeAsync(null);
                        }

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
