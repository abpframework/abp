using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Settings;
using Volo.Abp.Account.Web.Areas.Account.Controllers.Models;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using UserLoginInfo = Volo.Abp.Account.Web.Areas.Account.Controllers.Models.UserLoginInfo;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.Account.Web.Areas.Account.Controllers;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Controller]
[ControllerName("Login")]
[Area("account")]
[Route("api/account")]
public class AccountController : AbpControllerBase
{
    protected SignInManager<IdentityUser> SignInManager { get; }
    protected IdentityUserManager UserManager { get; }
    protected ISettingProvider SettingProvider { get; }
    protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }

    public AccountController(
        SignInManager<IdentityUser> signInManager,
        IdentityUserManager userManager,
        ISettingProvider settingProvider,
        IdentitySecurityLogManager identitySecurityLogManager,
        IOptions<IdentityOptions> identityOptions)
    {
        LocalizationResource = typeof(AccountResource);

        SignInManager = signInManager;
        UserManager = userManager;
        SettingProvider = settingProvider;
        IdentitySecurityLogManager = identitySecurityLogManager;
        IdentityOptions = identityOptions;
    }

    [HttpPost]
    [Route("login")]
    public virtual async Task<AbpLoginResult> Login(UserLoginInfo login)
    {
        await CheckLocalLoginAsync();

        ValidateLoginInfo(login);

        await ReplaceEmailToUsernameOfInputIfNeeds(login);
        var signInResult = await SignInManager.PasswordSignInAsync(
            login.UserNameOrEmailAddress,
            login.Password,
            login.RememberMe,
            true
        );

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = signInResult.ToIdentitySecurityLogAction(),
            UserName = login.UserNameOrEmailAddress
        });

        return GetAbpLoginResult(signInResult);
    }

    [HttpGet]
    [Route("logout")]
    public virtual async Task Logout()
    {
        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogActionConsts.Logout
        });

        await SignInManager.SignOutAsync();
    }

    [HttpPost]
    [Route("checkPassword")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public virtual Task<AbpLoginResult> CheckPasswordCompatible(UserLoginInfo login)
    {
        return CheckPassword(login);
    }

    [HttpPost]
    [Route("check-password")]
    public virtual async Task<AbpLoginResult> CheckPassword(UserLoginInfo login)
    {
        ValidateLoginInfo(login);

        await ReplaceEmailToUsernameOfInputIfNeeds(login);

        var identityUser = await UserManager.FindByNameAsync(login.UserNameOrEmailAddress);

        if (identityUser == null)
        {
            return new AbpLoginResult(LoginResultType.InvalidUserNameOrPassword);
        }

        await IdentityOptions.SetAsync();
        return GetAbpLoginResult(await SignInManager.CheckPasswordSignInAsync(identityUser, login.Password, true));
    }

    protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds(UserLoginInfo login)
    {
        if (!ValidationHelper.IsValidEmailAddress(login.UserNameOrEmailAddress))
        {
            return;
        }

        var userByUsername = await UserManager.FindByNameAsync(login.UserNameOrEmailAddress);
        if (userByUsername != null)
        {
            return;
        }

        var userByEmail = await UserManager.FindByEmailAsync(login.UserNameOrEmailAddress);
        if (userByEmail == null)
        {
            return;
        }

        login.UserNameOrEmailAddress = userByEmail.UserName;
    }

    private static AbpLoginResult GetAbpLoginResult(SignInResult result)
    {
        if (result.IsLockedOut)
        {
            return new AbpLoginResult(LoginResultType.LockedOut);
        }

        if (result.RequiresTwoFactor)
        {
            return new AbpLoginResult(LoginResultType.RequiresTwoFactor);
        }

        if (result.IsNotAllowed)
        {
            return new AbpLoginResult(LoginResultType.NotAllowed);
        }

        if (!result.Succeeded)
        {
            return new AbpLoginResult(LoginResultType.InvalidUserNameOrPassword);
        }

        return new AbpLoginResult(LoginResultType.Success);
    }

    protected virtual void ValidateLoginInfo(UserLoginInfo login)
    {
        if (login == null)
        {
            throw new ArgumentException(nameof(login));
        }

        if (login.UserNameOrEmailAddress.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(login.UserNameOrEmailAddress));
        }

        if (login.Password.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(login.Password));
        }
    }

    protected virtual async Task CheckLocalLoginAsync()
    {
        if (!await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
        {
            throw new UserFriendlyException(L["LocalLoginDisabledMessage"]);
        }
    }
}
