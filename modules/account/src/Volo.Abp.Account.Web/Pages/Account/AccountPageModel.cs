using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.Account.Web.Pages.Account;

public abstract class AccountPageModel : AbpPageModel
{
    public IAccountAppService AccountAppService { get; set; }
    public SignInManager<IdentityUser> SignInManager { get; set; }
    public IdentityUserManager UserManager { get; set; }
    public IdentitySecurityLogManager IdentitySecurityLogManager { get; set; }
    public IOptions<IdentityOptions> IdentityOptions { get; set; }
    public IExceptionToErrorInfoConverter ExceptionToErrorInfoConverter { get; set; }

    protected AccountPageModel()
    {
        LocalizationResourceType = typeof(AccountResource);
        ObjectMapperContext = typeof(AbpAccountWebModule);
    }

    protected virtual void CheckCurrentTenant(Guid? tenantId)
    {
        if (CurrentTenant.Id != tenantId)
        {
            throw new ApplicationException($"Current tenant is different than given tenant. CurrentTenant.Id: {CurrentTenant.Id}, given tenantId: {tenantId}");
        }
    }

    /// <summary>
    /// Checks the user before signing in without asking for the credentials of the user.
    /// </summary>
    /// <returns>Null if the user is allowed to sign in, otherwise the result why the user is denied.</returns>
    protected virtual async Task<SignInResult> CheckUserSignInAsync(IdentityUser user)
    {
        // The IdentityOptions property is hidden by the login page.
        await LazyServiceProvider.LazyGetRequiredService<IOptions<IdentityOptions>>().SetAsync();
        if (SignInManager is AbpSignInManager abpSignInManager)
        {
            return await abpSignInManager.CallPreSignInCheckAsync(user);
        }

        // The sign-in manager of the application may not be derived from AbpSignInManager.
        if (!user.IsActive ||
            user.ShouldChangePasswordOnNextLogin ||
            await UserManager.ShouldPeriodicallyChangePasswordAsync(user) ||
            !await SignInManager.CanSignInAsync(user))
        {
            return SignInResult.NotAllowed;
        }

        if (UserManager.SupportsUserLockout && await UserManager.IsLockedOutAsync(user))
        {
            return SignInResult.LockedOut;
        }

        return null;
    }

    protected virtual void CheckIdentityErrors(IdentityResult identityResult)
    {
        if (!identityResult.Succeeded)
        {
            throw new UserFriendlyException("Operation failed: " + identityResult.Errors.Select(e => $"[{e.Code}] {e.Description}").JoinAsString(", "));
        }

        //identityResult.CheckErrors(LocalizationManager); //TODO: Get from old Abp
    }

    protected virtual string GetLocalizeExceptionMessage(Exception exception)
    {
        if (exception is ILocalizeErrorMessage || exception is IHasErrorCode)
        {
            return ExceptionToErrorInfoConverter.Convert(exception, false).Message;
        }

        return exception.Message;
    }
}
