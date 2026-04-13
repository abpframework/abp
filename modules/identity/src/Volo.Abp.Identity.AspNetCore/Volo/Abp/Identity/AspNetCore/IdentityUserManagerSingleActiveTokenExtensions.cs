using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Volo.Abp.Identity.AspNetCore;

/// <summary>
/// Provides extension methods on <see cref="IdentityUserManager"/> for generating,
/// verifying, and invalidating single-active tokens managed by
/// <see cref="AbpSingleActiveTokenProvider"/>. These helpers live in the AspNetCore
/// layer because they depend on <see cref="AbpSingleActiveTokenProvider.InternalLoginProvider"/>.
/// </summary>
public static class IdentityUserManagerSingleActiveTokenExtensions
{
    /// <summary>
    /// Removes the stored password-reset token hash for <paramref name="user"/>,
    /// immediately invalidating any previously issued password-reset token.
    /// </summary>
    public static Task<IdentityResult> RemovePasswordResetTokenAsync(this IdentityUserManager manager, IdentityUser user)
    {
        var name = manager.Options.Tokens.PasswordResetTokenProvider + ":" + UserManager<IdentityUser>.ResetPasswordTokenPurpose;
        return manager.RemoveAuthenticationTokenAsync(user, AbpSingleActiveTokenProvider.InternalLoginProvider, name);
    }

    /// <summary>
    /// Removes the stored email-confirmation token hash for <paramref name="user"/>,
    /// immediately invalidating any previously issued email-confirmation token.
    /// </summary>
    public static Task<IdentityResult> RemoveEmailConfirmationTokenAsync(this IdentityUserManager manager, IdentityUser user)
    {
        var name = manager.Options.Tokens.EmailConfirmationTokenProvider + ":" + UserManager<IdentityUser>.ConfirmEmailTokenPurpose;
        return manager.RemoveAuthenticationTokenAsync(user, AbpSingleActiveTokenProvider.InternalLoginProvider, name);
    }

    /// <summary>
    /// Removes the stored change-email token hash for <paramref name="user"/>,
    /// immediately invalidating any previously issued change-email token for <paramref name="newEmail"/>.
    /// </summary>
    public static Task<IdentityResult> RemoveChangeEmailTokenAsync(this IdentityUserManager manager, IdentityUser user, string newEmail)
    {
        var name = manager.Options.Tokens.ChangeEmailTokenProvider + ":" + UserManager<IdentityUser>.GetChangeEmailTokenPurpose(newEmail);
        return manager.RemoveAuthenticationTokenAsync(user, AbpSingleActiveTokenProvider.InternalLoginProvider, name);
    }

    /// <summary>
    /// Generates a new email login token for <paramref name="user"/>.
    /// Any previously issued email login token is automatically invalidated.
    /// </summary>
    public static Task<string> GenerateEmailLoginTokenAsync(this IdentityUserManager manager, IdentityUser user)
    {
        return manager.GenerateUserTokenAsync(user, AbpEmailLoginTokenProvider.ProviderName, AbpEmailLoginTokenPurposes.EmailLogin);
    }

    /// <summary>
    /// Verifies the email login token for <paramref name="user"/>.
    /// </summary>
    public static Task<bool> VerifyEmailLoginTokenAsync(this IdentityUserManager manager, IdentityUser user, string token)
    {
        return manager.VerifyUserTokenAsync(user, AbpEmailLoginTokenProvider.ProviderName, AbpEmailLoginTokenPurposes.EmailLogin, token);
    }

    /// <summary>
    /// Removes the stored email login token hash for <paramref name="user"/>,
    /// immediately invalidating any previously issued email login token.
    /// </summary>
    public static Task<IdentityResult> RemoveEmailLoginTokenAsync(this IdentityUserManager manager, IdentityUser user)
    {
        var name = AbpEmailLoginTokenProvider.ProviderName + ":" + AbpEmailLoginTokenPurposes.EmailLogin;
        return manager.RemoveAuthenticationTokenAsync(user, AbpSingleActiveTokenProvider.InternalLoginProvider, name);
    }

}
