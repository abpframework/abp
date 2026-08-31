using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Volo.Abp.Identity.AspNetCore;

/// <summary>
/// Provides extension methods on <see cref="IdentityUserManager"/> for invalidating
/// single-active tokens managed by <see cref="AbpSingleActiveTokenProvider"/>.
/// </summary>
public static class IdentityUserManagerSingleActiveTokenExtensions
{
    /// <summary>
    /// Removes the stored password-reset token hash for <paramref name="user"/>,
    /// immediately invalidating any previously issued password-reset token.
    /// </summary>
    public static Task<IdentityResult> RemovePasswordResetTokenAsync(this IdentityUserManager manager, IdentityUser user)
    {
        return RemoveStoredTokenAsync(
            manager,
            user,
            manager.Options.Tokens.PasswordResetTokenProvider,
            UserManager<IdentityUser>.ResetPasswordTokenPurpose);
    }

    /// <summary>
    /// Removes the stored email-confirmation token hash for <paramref name="user"/>,
    /// immediately invalidating any previously issued email-confirmation token.
    /// </summary>
    public static Task<IdentityResult> RemoveEmailConfirmationTokenAsync(this IdentityUserManager manager, IdentityUser user)
    {
        return RemoveStoredTokenAsync(
            manager,
            user,
            manager.Options.Tokens.EmailConfirmationTokenProvider,
            UserManager<IdentityUser>.ConfirmEmailTokenPurpose);
    }

    /// <summary>
    /// Removes the stored change-email token hash for <paramref name="user"/>,
    /// immediately invalidating any previously issued change-email token for <paramref name="newEmail"/>.
    /// </summary>
    public static Task<IdentityResult> RemoveChangeEmailTokenAsync(this IdentityUserManager manager, IdentityUser user, string newEmail)
    {
        return RemoveStoredTokenAsync(
            manager,
            user,
            manager.Options.Tokens.ChangeEmailTokenProvider,
            UserManager<IdentityUser>.GetChangeEmailTokenPurpose(newEmail));
    }

    /// <summary>
    /// Removes the stored link-user token hash for <paramref name="user"/>,
    /// immediately invalidating any previously issued link-user token.
    /// </summary>
    public static Task<IdentityResult> RemoveLinkUserTokenAsync(this IdentityUserManager manager, IdentityUser user)
    {
        return RemoveLinkUserTokenAsync(manager, user, LinkUserTokenProviderConsts.LinkUserTokenPurpose);
    }

    /// <summary>
    /// Removes the stored link-user token hash for <paramref name="user"/> and the given <paramref name="purpose"/>,
    /// immediately invalidating any previously issued link-user token for that purpose.
    /// </summary>
    public static Task<IdentityResult> RemoveLinkUserTokenAsync(this IdentityUserManager manager, IdentityUser user, string purpose)
    {
        return RemoveStoredTokenAsync(
            manager,
            user,
            LinkUserTokenProviderConsts.LinkUserTokenProviderName,
            purpose);
    }

    private static Task<IdentityResult> RemoveStoredTokenAsync(
        IdentityUserManager manager,
        IdentityUser user,
        string providerKey,
        string purpose)
    {
        return manager.RemoveAuthenticationTokenAsync(
            user,
            AbpSingleActiveTokenProvider.InternalLoginProvider,
            GetStoredTokenName(manager, providerKey, purpose));
    }

    /// <summary>
    /// The hash is stored under the provider's options <c>Name</c>, which is configurable and therefore
    /// not necessarily the key the provider is registered under.
    /// </summary>
    private static string GetStoredTokenName(IdentityUserManager manager, string providerKey, string purpose)
    {
        var descriptor = manager.Options.Tokens.ProviderMap.GetOrDefault(providerKey);
        var provider = descriptor?.ProviderInstance ?? (descriptor != null
            ? manager.ServiceProvider.GetService(descriptor.ProviderType)
            : null);

        if (provider is not AbpSingleActiveTokenProvider singleActiveTokenProvider)
        {
            throw new AbpException(
                $"The '{providerKey}' token provider is not an {nameof(AbpSingleActiveTokenProvider)}, so it does not " +
                $"store a token hash that can be removed. This happens when the key has no provider at all, when " +
                $"the ABP token providers are turned off " +
                $"through {nameof(AbpIdentityTokenProviderOptions)}.{nameof(AbpIdentityTokenProviderOptions.UseAbpTokenProviders)} " +
                $"or when the key was re-registered with another provider, including one registered for a " +
                $"second user type.");
        }

        return singleActiveTokenProvider.Name + ":" + purpose;
    }
}
