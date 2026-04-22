using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpSignInManager : SignInManager<IdentityUser>
{
    protected AbpIdentityOptions AbpOptions { get; }
    protected ISettingProvider SettingProvider { get; }
    protected IdentityUserManager IdentityUserManager { get; }
    protected ICurrentTenant CurrentTenant { get; }

    public AbpSignInManager(
        IdentityUserManager userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<IdentityUser>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<IdentityUser> confirmation,
        IOptions<AbpIdentityOptions> options,
        ISettingProvider settingProvider,
        ICurrentTenant currentTenant) : base(
        userManager,
        contextAccessor,
        claimsFactory,
        optionsAccessor,
        logger,
        schemes,
        confirmation)
    {
        AbpOptions = options.Value;
        SettingProvider = settingProvider;
        IdentityUserManager = userManager;
        CurrentTenant = currentTenant;
    }

    public override async Task<SignInResult> PasswordSignInAsync(
        string userName,
        string password,
        bool isPersistent,
        bool lockoutOnFailure)
    {
        IdentityUser user;
        foreach (var externalLoginProviderInfo in AbpOptions.ExternalLoginProviders.Values)
        {
            var externalLoginProvider = (IExternalLoginProvider)Context.RequestServices
                .GetRequiredService(externalLoginProviderInfo.Type);

            if (await externalLoginProvider.TryAuthenticateAsync(userName, password))
            {
                user = await FindByNameAsync(userName);
                if (user == null)
                {
                    if (externalLoginProvider is IExternalLoginProviderWithPassword externalLoginProviderWithPassword)
                    {
                        user = await externalLoginProviderWithPassword.CreateUserAsync(userName, externalLoginProviderInfo.Name, password);
                    }
                    else
                    {
                        user = await externalLoginProvider.CreateUserAsync(userName, externalLoginProviderInfo.Name);
                    }
                }
                else
                {
                    using (CurrentTenant.Change(user.TenantId))
                    {
                        if (externalLoginProvider is IExternalLoginProviderWithPassword externalLoginProviderWithPassword)
                        {
                            await externalLoginProviderWithPassword.UpdateUserAsync(user, externalLoginProviderInfo.Name, password);
                        }
                        else
                        {
                            await externalLoginProvider.UpdateUserAsync(user, externalLoginProviderInfo.Name);
                        }
                    }
                }

                using (CurrentTenant.Change(user.TenantId))
                {
                    return await SignInOrTwoFactorAsync(user, isPersistent);
                }
            }
        }

        user = await FindByNameAsync(userName);
        if (user == null)
        {
            return SignInResult.Failed;
        }

        using (CurrentTenant.Change(user.TenantId))
        {
            return await PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }
    }

    public override async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor)
    {
        var user = await FindByLoginAsync(loginProvider, providerKey);
        if (user == null)
        {
            return SignInResult.Failed;
        }

        var error = await PreSignInCheck(user);
        if (error != null)
        {
            return error;
        }
        return await SignInOrTwoFactorAsync(user, isPersistent, loginProvider, bypassTwoFactor);
    }

    public virtual async Task<IdentityUser> FindByEmailAsync(string email)
    {
        return await IdentityUserManager.FindSharedUserByEmailAsync(email);
    }

    public virtual async Task<IdentityUser> FindByNameAsync(string userName)
    {
        return await IdentityUserManager.FindSharedUserByNameAsync(userName);
    }

    public virtual async Task<IdentityUser> FindByLoginAsync(string loginProvider, string providerKey)
    {
        return await IdentityUserManager.FindSharedUserByLoginAsync(loginProvider, providerKey);
    }

    /// <summary>
    /// This is to call the protection method PreSignInCheck
    /// </summary>
    /// <param name="user">The user</param>
    /// <returns>Null if the user should be allowed to sign in, otherwise the SignInResult why they should be denied.</returns>
    public virtual async Task<SignInResult> CallPreSignInCheckAsync(IdentityUser user)
    {
        return await PreSignInCheck(user);
    }

    protected override async Task<SignInResult> PreSignInCheck(IdentityUser user)
    {
        if (!user.IsActive)
        {
            Logger.LogWarning($"The user is not active therefore cannot login! (username: \"{user.UserName}\", id:\"{user.Id}\")");
            return SignInResult.NotAllowed;
        }

        if (user.ShouldChangePasswordOnNextLogin)
        {
            Logger.LogWarning($"The user should change password! (username: \"{user.UserName}\", id:\"{user.Id}\")");
            return SignInResult.NotAllowed;
        }

        if (await IdentityUserManager.ShouldPeriodicallyChangePasswordAsync(user))
        {
            return SignInResult.NotAllowed;
        }

        return await base.PreSignInCheck(user);
    }

    /// <summary>
    /// This is to call the protection method SignInOrTwoFactorAsync
    /// </summary>
    /// <param name="user"></param>
    /// <param name="isPersistent"></param>
    /// <param name="loginProvider"></param>
    /// <param name="bypassTwoFactor"></param>
    /// <returns></returns>
    public virtual async Task<SignInResult> CallSignInOrTwoFactorAsync(IdentityUser user, bool isPersistent, string loginProvider = null, bool bypassTwoFactor = false)
    {
        return await base.SignInOrTwoFactorAsync(user, isPersistent, loginProvider, bypassTwoFactor);
    }
}
