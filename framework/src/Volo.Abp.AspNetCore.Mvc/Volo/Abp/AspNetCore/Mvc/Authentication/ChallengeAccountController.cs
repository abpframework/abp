using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.Authentication;

public abstract class ChallengeAccountController : AbpController
{
    protected string[] ChallengeAuthenticationSchemas { get; }
    protected string AuthenticationType { get; }
    protected string[] ForbidSchemes { get; }

    protected ChallengeAccountController(string[] challengeAuthenticationSchemas = null)
    {
        ChallengeAuthenticationSchemas = challengeAuthenticationSchemas ?? new[] { "oidc" };
        AuthenticationType = "Identity.Application";
        ForbidSchemes = Array.Empty<string>();
    }

    [HttpGet]
    public virtual ActionResult Login(string returnUrl = "", string returnUrlHash = "")
    {
        if (CurrentUser.IsAuthenticated)
        {
            return RedirectSafely(returnUrl, returnUrlHash);
        }

        return Challenge(new AuthenticationProperties { RedirectUri = GetRedirectUrl(returnUrl, returnUrlHash) }, ChallengeAuthenticationSchemas);
    }

    [HttpGet]
    public virtual async Task<ActionResult> Logout(string returnUrl = "", string returnUrlHash = "")
    {
        await HttpContext.SignOutAsync();

        if (HttpContext.User.Identity?.AuthenticationType == AuthenticationType)
        {
            return RedirectSafely(returnUrl, returnUrlHash);
        }

        return SignOut(new AuthenticationProperties { RedirectUri = GetRedirectUrl(returnUrl, returnUrlHash) }, ChallengeAuthenticationSchemas);
    }

    [HttpGet]
    public virtual async Task<IActionResult> FrontChannelLogout(string sid)
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var currentSid = User.FindFirst("sid")?.Value ?? string.Empty;
            if (string.Equals(currentSid, sid, StringComparison.Ordinal))
            {
                await Logout();
            }
        }

        return NoContent();
    }

    [HttpGet]
    public virtual Task<IActionResult> AccessDenied(string returnUrl = "", string returnUrlHash = "")
    {
        return Task.FromResult<IActionResult>(Challenge(
            new AuthenticationProperties
            {
                RedirectUri = GetRedirectUrl(returnUrl, returnUrlHash)
            },
            ForbidSchemes.IsNullOrEmpty()
                ? new[]
                {
                    HttpContext.RequestServices.GetRequiredService<IOptions<AuthenticationOptions>>().Value.DefaultForbidScheme
                }
                : ForbidSchemes
        ));
    }
}
