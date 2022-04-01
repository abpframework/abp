using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Volo.Abp.OpenIddict.Controllers;

public partial class TokenController
{
    protected virtual async Task<IActionResult> HandleAuthorizationCodeAsync(OpenIddictRequest request)
    {
        // Retrieve the claims principal stored in the authorization code/device code/refresh token.
        var principal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;

        // Retrieve the user profile corresponding to the authorization code/refresh token.
        // Note: if you want to automatically invalidate the authorization code/refresh token
        // when the user password/roles change, use the following line instead:
        // var user = _signInManager.ValidateSecurityStampAsync(info.Principal);
        var user = await UserManager.GetUserAsync(principal);
        if (user == null)
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = L["TheTokenIsNoLongerValid"]
                }));
        }

        // Ensure the user is still allowed to sign in.
        if (!await SignInManager.CanSignInAsync(user))
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = L["TheUserIsNoLongerAllowedToSignIn"]
                }));
        }

        await SetClaimsDestinationsAsync(principal);

        // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
