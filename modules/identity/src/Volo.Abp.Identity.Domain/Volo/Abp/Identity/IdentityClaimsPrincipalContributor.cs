using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Identity;

public class IdentityClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
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
                var roleManager = context.ServiceProvider.GetRequiredService<IdentityRoleManager>();
                var identityOptions = context.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>().Value;

                if (!user.UserName.IsNullOrWhiteSpace())
                {
                    identity.AddIfNotContains(new Claim(identityOptions.ClaimsIdentity.UserNameClaimType, user.UserName));
                }

                if (userManager.SupportsUserRole)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    foreach (var roleName in roles)
                    {
                        if (!identity.HasClaim(x => x.Type == identityOptions.ClaimsIdentity.RoleClaimType && x.Value == roleName))
                        {
                            identity.AddClaim(new Claim(identityOptions.ClaimsIdentity.RoleClaimType, roleName));
                            if (roleManager.SupportsRoleClaims)
                            {
                                var role = await roleManager.FindByNameAsync(roleName);
                                if (role != null)
                                {
                                    identity.AddClaims(await roleManager.GetClaimsAsync(role));
                                }
                            }
                        }
                    }
                }

                if (!user.Name.IsNullOrWhiteSpace())
                {
                    identity.AddIfNotContains(new Claim(AbpClaimTypes.Name, user.Name));
                }

                if (!user.Surname.IsNullOrWhiteSpace())
                {
                    identity.AddIfNotContains(new Claim(AbpClaimTypes.SurName, user.Surname));
                }

                if (!user.PhoneNumber.IsNullOrWhiteSpace())
                {
                    identity.AddIfNotContains(new Claim(AbpClaimTypes.PhoneNumber, user.PhoneNumber));
                }

                identity.AddIfNotContains(new Claim(AbpClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed.ToString()));

                if (!user.Email.IsNullOrWhiteSpace())
                {
                    identity.AddIfNotContains(new Claim(AbpClaimTypes.Email, user.Email));
                }

                identity.AddIfNotContains(new Claim(AbpClaimTypes.EmailVerified, user.EmailConfirmed.ToString()));
            }
        }
    }
}
