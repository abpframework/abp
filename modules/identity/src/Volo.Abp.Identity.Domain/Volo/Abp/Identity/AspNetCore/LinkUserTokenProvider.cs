using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Identity;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity.AspNetCore;

/// <summary>
/// Link-user token provider that enforces, per purpose, only the most recently issued
/// token to be valid, with a configurable expiration period.
/// </summary>
public class LinkUserTokenProvider : AbpSingleActiveTokenProvider
{
    public LinkUserTokenProvider(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<AbpLinkUserTokenProviderOptions> options,
        ILogger<AbpSingleActiveTokenProvider> logger,
        IIdentityUserRepository userRepository,
        ICancellationTokenProvider cancellationTokenProvider)
        : base(dataProtectionProvider, options, logger, userRepository, cancellationTokenProvider)
    {
    }
}
