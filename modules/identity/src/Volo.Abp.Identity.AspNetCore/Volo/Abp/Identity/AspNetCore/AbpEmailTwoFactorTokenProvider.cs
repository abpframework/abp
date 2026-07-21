using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Identity;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Volo.Abp.Identity.AspNetCore;

/// <summary>
/// Single-use email 2FA code provider; replaces the ASP.NET Core Identity TOTP-based
/// <c>EmailTokenProvider&lt;TUser&gt;</c> under <see cref="TokenOptions.DefaultEmailProvider"/>.
/// </summary>
public class AbpEmailTwoFactorTokenProvider : AbpTwoFactorTokenProvider
{
    public const string ProviderName = "AbpEmailTwoFactor";

    public override string Name => ProviderName;

    public AbpEmailTwoFactorTokenProvider(
        IOptions<AbpEmailTwoFactorTokenProviderOptions> options,
        IIdentityUserRepository userRepository,
        ICancellationTokenProvider cancellationTokenProvider,
        IClock clock,
        IDataProtectionProvider dataProtectionProvider)
        : base(options.Value, userRepository, cancellationTokenProvider, clock, dataProtectionProvider)
    {
    }

    public override async Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<IdentityUser> manager, IdentityUser user)
    {
        var email = await manager.GetEmailAsync(user);
        return !string.IsNullOrWhiteSpace(email) && await manager.IsEmailConfirmedAsync(user);
    }
}
