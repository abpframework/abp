using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Volo.Abp.OpenIddict.Controllers;

[Route("connect/userinfo")]
[Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
public class UserInfoController : OpenIdDictControllerBase
{
    [HttpGet]
    [HttpPost]
    [Produces("application/json")]
    public async Task<IActionResult> Userinfo()
    {
        var user = await UserManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidToken,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The specified access token is bound to an account that no longer exists." //TODO: L
                }));
        }

        var claims = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
            [OpenIddictConstants.Claims.Subject] = await UserManager.GetUserIdAsync(user)
        };

        if (User.HasScope(OpenIddictConstants.Scopes.Email))
        {
            claims[OpenIddictConstants.Claims.Email] = await UserManager.GetEmailAsync(user);
            claims[OpenIddictConstants.Claims.EmailVerified] = await UserManager.IsEmailConfirmedAsync(user);
        }

        if (User.HasScope(OpenIddictConstants.Scopes.Phone))
        {
            claims[OpenIddictConstants.Claims.PhoneNumber] = await UserManager.GetPhoneNumberAsync(user);
            claims[OpenIddictConstants.Claims.PhoneNumberVerified] = await UserManager.IsPhoneNumberConfirmedAsync(user);
        }

        if (User.HasScope(OpenIddictConstants.Scopes.Roles))
        {
            claims[OpenIddictConstants.Claims.Role] = await UserManager.GetRolesAsync(user);
        }

        // Note: the complete list of standard claims supported by the OpenID Connect specification
        // can be found here: http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims

        return Ok(claims);
    }
}
