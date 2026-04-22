using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity.AspNetCore;

[Route("api/signin-test")]
public class SignInTestController : AbpController
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public SignInTestController(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [Route("password")]
    public async Task<ActionResult> PasswordLogin(string userName, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(
            userName,
            password,
            false,
            false
        );

        return Content(result.ToString());
    }

    [Route("write-two-factor-cookie")]
    public async Task<ActionResult> WriteTwoFactorCookie(string userId)
    {
        var identity = new ClaimsIdentity(IdentityConstants.TwoFactorUserIdScheme);
        identity.AddClaim(new Claim(ClaimTypes.Name, userId));
        await HttpContext.SignInAsync(IdentityConstants.TwoFactorUserIdScheme, new ClaimsPrincipal(identity));
        return Content("OK");
    }

    [Route("get-two-factor-user")]
    public async Task<ActionResult> GetTwoFactorUser()
    {
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        return Content(user?.Id.ToString() ?? "null");
    }

    [Route("two-factor-signin")]
    public async Task<ActionResult> TwoFactorSignIn(string provider, string code)
    {
        var result = await _signInManager.TwoFactorSignInAsync(provider, code, false, false);
        return Content(result.ToString());
    }

    [Route("two-factor-recovery-signin")]
    public async Task<ActionResult> TwoFactorRecoverySignIn(string recoveryCode)
    {
        var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);
        return Content(result.ToString());
    }

    [Route("external-login-signin")]
    public async Task<ActionResult> ExternalLoginSignIn(string loginProvider, string providerKey)
    {
        var result = await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, false, false);
        return Content(result.ToString());
    }
}
