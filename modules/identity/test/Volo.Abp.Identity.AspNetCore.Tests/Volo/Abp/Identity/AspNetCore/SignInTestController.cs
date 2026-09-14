using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Users;

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

    [Route("current-user")]
    public ActionResult GetCurrentUser()
    {
        return Content(CurrentUser.IsAuthenticated ? CurrentUser.UserName + "|" + CurrentUser.FindSessionId() : "anonymous");
    }

    [Route("switch-account")]
    public async Task<ActionResult> SwitchAccount(string userName)
    {
        // Account switch (LinkLogin / impersonation): sign out the current identity and sign in
        // as another user within the same request.
        await _signInManager.SignOutAsync();
        var user = await _signInManager.UserManager.FindByNameAsync(userName);
        await _signInManager.SignInAsync(user, isPersistent: false);
        return Content("Succeeded");
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
