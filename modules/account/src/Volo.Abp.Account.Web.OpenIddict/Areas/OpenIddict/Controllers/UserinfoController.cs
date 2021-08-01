using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Volo.Abp.Account.Web.Areas.OpenIddict.Controllers
{
    [Area("openiddict")]
    [ControllerName("OpenIddictUserinfo")]
    public class UserinfoController : AbpController
    {
        private readonly IdentityUserManager _userManager;

        public UserinfoController(IdentityUserManager userManager)
            => _userManager = userManager;

        [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
        [Route("~/connect/userinfo")]
        [HttpGet, HttpPost]
        [IgnoreAntiforgeryToken]
        [Produces("application/json")]
        public async Task<IActionResult> Userinfo()
        {
            var tenandIdValue = User?.FindFirst(AbpClaimTypes.TenantId)?.Value;
            Guid? tenantId = null;
            if (Guid.TryParse(tenandIdValue, out var t))
            {
                tenantId = t;
            }

            using var _ = CurrentTenant.Change(tenantId);

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Challenge(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidToken,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The specified access token is bound to an account that no longer exists."
                    }));
            }
            var username = await _userManager.GetUserNameAsync(user);

            var claims = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
                [Claims.Subject] = await _userManager.GetUserIdAsync(user),
                [Claims.PreferredUsername] = username,
                [Claims.Name] = username,
            };

            if (User.HasScope(Scopes.Email) && _userManager.SupportsUserEmail)
            {
                claims[Claims.Email] = await _userManager.GetEmailAsync(user);
                claims[Claims.EmailVerified] = await _userManager.IsEmailConfirmedAsync(user);
            }

            if (User.HasScope(Scopes.Phone) && _userManager.SupportsUserPhoneNumber)
            {
                claims[Claims.PhoneNumber] = await _userManager.GetPhoneNumberAsync(user);
                claims[Claims.PhoneNumberVerified] = await _userManager.IsPhoneNumberConfirmedAsync(user);
            }

            if (User.HasScope(Scopes.Roles))
            {
                claims[Claims.Role] = await _userManager.GetRolesAsync(user);
            }

            if (user.TenantId.HasValue)
            {
                claims[AbpClaimTypes.TenantId] = user.TenantId?.ToString();
            }

            // Note: the complete list of standard claims supported by the OpenID Connect specification
            // can be found here: http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims

            if (!user.Name.IsNullOrEmpty())
            {
                claims[Claims.GivenName] = user.Name;
            }

            if (!user.Surname.IsNullOrEmpty())
            {
                claims[Claims.FamilyName] = user.Surname;
            }

            return Ok(claims);
        }
    }
}
