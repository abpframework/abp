using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.OpenIddict;

/// <summary>
/// https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1627
/// https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/05e02b5e0383be40e45c667c12f6667d38e33fcc/src/System.IdentityModel.Tokens.Jwt/ClaimTypeMapping.cs#L52
/// </summary>
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
                identity.AddIfNotContains(new Claim(JwtRegisteredClaimNames.UniqueName, usernameClaim.Value));
            }
        }

        return Task.CompletedTask;
    }
}
