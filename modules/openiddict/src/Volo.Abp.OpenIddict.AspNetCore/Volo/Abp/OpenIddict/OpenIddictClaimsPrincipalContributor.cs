using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.OpenIddict;

public class OpenIddictClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
{
    public Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        if (identity != null)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>().Value;
            var usernameClaim = identity.FindFirst(options.ClaimsIdentity.UserNameClaimType);
            if (usernameClaim != null)
            {
                identity.AddIfNotContains(new Claim(OpenIddictConstants.Claims.PreferredUsername, usernameClaim.Value));
                identity.AddIfNotContains(new Claim(JwtRegisteredClaimNames.UniqueName, usernameClaim.Value));
            }

            var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            if (httpContext != null)
            {
                var clientId = httpContext.GetOpenIddictServerRequest()?.ClientId;
                if (clientId != null)
                {
                    identity.AddClaim(OpenIddictConstants.Claims.ClientId, clientId, OpenIddictConstants.Destinations.AccessToken);
                }
            }
        }

        return Task.CompletedTask;
    }
}
