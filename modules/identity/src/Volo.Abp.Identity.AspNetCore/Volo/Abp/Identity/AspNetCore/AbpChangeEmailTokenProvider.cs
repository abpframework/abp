using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Identity;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity.AspNetCore;

/// <summary>
/// Change email token provider that enforces only the most recently issued
/// token to be valid, with a configurable expiration period.
/// </summary>
public class AbpChangeEmailTokenProvider : AbpSingleActiveTokenProvider
{
    public const string ProviderName = "AbpChangeEmail";

    public AbpChangeEmailTokenProvider(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<AbpChangeEmailTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<IdentityUser>> logger,
        IIdentityUserRepository userRepository,
        ICancellationTokenProvider cancellationTokenProvider)
        : base(dataProtectionProvider, options, logger, userRepository, cancellationTokenProvider)
    {
    }
}
