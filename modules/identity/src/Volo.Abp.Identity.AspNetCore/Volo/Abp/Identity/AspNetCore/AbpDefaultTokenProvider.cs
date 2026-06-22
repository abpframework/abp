using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Identity;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity.AspNetCore;

/// <summary>
/// Replaces ASP.NET Identity's default <see cref="DataProtectorTokenProvider{IdentityUser}"/>
/// registered under <see cref="TokenOptions.DefaultProvider"/> ("Default"). Used by callers such
/// as the IdentityServer / OpenIddict token endpoints to issue short-lived challenge tokens
/// (RequiresTwoFactor, ShouldChangePasswordOnNextLogin, PeriodicallyChangePassword)
/// and consumed back by Account / SendSecurityCode.
/// Enforces, per purpose, a single active token to be valid.
/// </summary>
public class AbpDefaultTokenProvider : AbpSingleActiveTokenProvider
{
    public AbpDefaultTokenProvider(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<AbpDefaultTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<IdentityUser>> logger,
        IIdentityUserRepository userRepository,
        ICancellationTokenProvider cancellationTokenProvider)
        : base(dataProtectionProvider, options, logger, userRepository, cancellationTokenProvider)
    {
    }
}
