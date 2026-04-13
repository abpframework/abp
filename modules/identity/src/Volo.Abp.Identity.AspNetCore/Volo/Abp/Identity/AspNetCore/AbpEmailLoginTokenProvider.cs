using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity.AspNetCore;

/// <summary>
/// Email login token provider that enforces only the most recently issued
/// token to be valid, with a configurable expiration period.
/// </summary>
public class AbpEmailLoginTokenProvider : AbpSingleActiveTokenProvider
{
    public const string ProviderName = "AbpEmailLogin";

    public AbpEmailLoginTokenProvider(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<AbpEmailLoginTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<IdentityUser>> logger,
        IIdentityUserRepository userRepository,
        ICancellationTokenProvider cancellationTokenProvider)
        : base(dataProtectionProvider, options, logger, userRepository, cancellationTokenProvider)
    {
    }
}
