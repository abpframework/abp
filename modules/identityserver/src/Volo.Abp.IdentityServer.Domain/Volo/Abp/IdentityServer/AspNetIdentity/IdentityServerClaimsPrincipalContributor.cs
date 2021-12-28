using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.IdentityServer.AspNetIdentity;

public class IdentityServerClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
{
    public virtual async Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();

        var userId = identity?.FindUserId();
        if (userId == null)
        {
            return;
        }

        using (context.ServiceProvider.GetRequiredService<ICurrentTenant>().Change(identity.FindTenantId()))
        {
            var userManager = context.ServiceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var username = await userManager.GetUserNameAsync(user);
                var usernameClaim = identity.FindFirst(claim =>
                    claim.Type == userManager.Options.ClaimsIdentity.UserNameClaimType && claim.Value == username);
                if (usernameClaim != null)
                {
                    identity.RemoveClaim(usernameClaim);
                    identity.AddIfNotContains(new Claim(JwtClaimTypes.PreferredUserName, username));

                    //https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1627
                    //https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/05e02b5e0383be40e45c667c12f6667d38e33fcc/src/System.IdentityModel.Tokens.Jwt/ClaimTypeMapping.cs#L52
                    identity.AddIfNotContains(new Claim(JwtRegisteredClaimNames.UniqueName, username));
                }

                if (!identity.HasClaim(x => x.Type == JwtClaimTypes.Name))
                {
                    identity.AddIfNotContains(new Claim(JwtClaimTypes.Name, username));
                }

                if (userManager.SupportsUserEmail)
                {
                    var email = await userManager.GetEmailAsync(user);
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        identity.AddIfNotContains(new Claim(JwtClaimTypes.Email, email));
                        identity.AddIfNotContains(new Claim(JwtClaimTypes.EmailVerified,
                            await userManager.IsEmailConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean));
                    }
                }

                if (userManager.SupportsUserPhoneNumber)
                {
                    var phoneNumber = await userManager.GetPhoneNumberAsync(user);
                    if (!string.IsNullOrWhiteSpace(phoneNumber))
                    {
                        identity.AddIfNotContains(new Claim(JwtClaimTypes.PhoneNumber, phoneNumber));
                        identity.AddIfNotContains(new Claim(JwtClaimTypes.PhoneNumberVerified,
                            await userManager.IsPhoneNumberConfirmedAsync(user) ? "true" : "false",
                            ClaimValueTypes.Boolean));
                    }
                }

                if (!user.Name.IsNullOrEmpty())
                {
                    identity.AddIfNotContains(new Claim(JwtClaimTypes.GivenName, user.Name));
                }

                if (!user.Surname.IsNullOrEmpty())
                {
                    identity.AddIfNotContains(new Claim(JwtClaimTypes.FamilyName, user.Surname));
                }
            }
        }
    }
}
