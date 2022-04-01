using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;

namespace Volo.Abp.OpenIddict.Controllers;

[Route("connect/logout")]
public class LogoutController : AbpOpenIdDictControllerBase
{
    [HttpGet]
    public virtual Task<IActionResult> GetAsync()
    {
        return Task.FromResult<IActionResult>(View("Logout"));
    }

    [HttpPost]
    public virtual async Task<IActionResult> HandleAcceptAsync()
    {
        // Ask ASP.NET Core Identity to delete the local and external cookies created
        // when the user agent is redirected from the external identity provider
        // after a successful authentication flow (e.g Google or Facebook).
        await SignInManager.SignOutAsync();

        // Returning a SignOutResult will ask OpenIddict to redirect the user agent
        // to the post_logout_redirect_uri specified by the client application or to
        // the RedirectUri specified in the authentication properties if none was set.
        return SignOut(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: new AuthenticationProperties
            {
                RedirectUri = "/"
            });
    }
}
