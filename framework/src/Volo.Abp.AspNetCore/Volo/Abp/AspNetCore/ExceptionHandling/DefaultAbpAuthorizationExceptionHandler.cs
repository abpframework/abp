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
        public virtual async Task HandleAsync(AbpAuthorizationException exception, HttpContext httpContext)
        {
            var handlerOptions = httpContext.RequestServices.GetRequiredService<IOptions<AbpAuthorizationExceptionHandlerOptions>>().Value;
            var isAuthenticated = httpContext.User.Identity?.IsAuthenticated ?? false;
            var authenticationSchemeProvider = httpContext.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();

            AuthenticationScheme scheme = null;

            if (!handlerOptions.AuthenticationScheme.IsNullOrWhiteSpace())
            {
                scheme = await authenticationSchemeProvider.GetSchemeAsync(handlerOptions.AuthenticationScheme);
                if (scheme == null)
                {
                    throw new AbpException($"No authentication scheme named {handlerOptions.AuthenticationScheme} was found.");
                }
            }
            else
            {
                if (isAuthenticated)
                {
                    scheme = await authenticationSchemeProvider.GetDefaultForbidSchemeAsync();
                    if (scheme == null)
                    {
                        throw new AbpException($"There was no DefaultForbidScheme found.");
                    }
                }
                else
                {
                    scheme = await authenticationSchemeProvider.GetDefaultChallengeSchemeAsync();
                    if (scheme == null)
                    {
                        throw new AbpException($"There was no DefaultChallengeScheme found.");
                    }
                }
            }

            var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            var handler = await handlers.GetHandlerAsync(httpContext, scheme.Name);
            if (handler == null)
            {
                throw new AbpException($"No handler of {scheme.Name} was found.");
            }

            if (isAuthenticated)
            {
                await handler.ForbidAsync(null);
            }
            else
            {
                await handler.ChallengeAsync(null);
            }
        }
    }
}
