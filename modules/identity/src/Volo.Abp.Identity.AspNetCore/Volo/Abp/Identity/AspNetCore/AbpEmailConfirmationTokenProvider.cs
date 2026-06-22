using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Identity;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity.AspNetCore;

/// <summary>
/// Email confirmation token provider that enforces only the most recently issued
/// token to be valid, with a configurable expiration period.
/// Token reuse is bounded by the token expiry and the single-active policy:
/// generating a new token overwrites the stored hash, invalidating all previous tokens.
/// <para>
/// Unlike password-reset and change-email flows, <see cref="Microsoft.AspNetCore.Identity.UserManager{TUser}.ConfirmEmailAsync"/>
/// does <b>not</b> update the security stamp, so the token hash is NOT automatically
/// invalidated after a successful confirmation. Callers that require single-use semantics
/// must explicitly revoke the hash after confirmation:
/// <code>
/// var result = await userManager.ConfirmEmailAsync(user, token);
/// if (result.Succeeded)
///     await userManager.RemoveEmailConfirmationTokenAsync(user);
/// </code>
/// </para>
/// </summary>
public class AbpEmailConfirmationTokenProvider : AbpSingleActiveTokenProvider
{
    public const string ProviderName = "AbpEmailConfirmation";

    public AbpEmailConfirmationTokenProvider(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<AbpEmailConfirmationTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<IdentityUser>> logger,
        IIdentityUserRepository userRepository,
        ICancellationTokenProvider cancellationTokenProvider)
        : base(dataProtectionProvider, options, logger, userRepository, cancellationTokenProvider)
    {
    }
}
