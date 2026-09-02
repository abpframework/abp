using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Identity;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Volo.Abp.Identity;

/// <summary>
/// Single-use phone 2FA code provider; replaces the ASP.NET Core Identity TOTP-based
/// <c>PhoneNumberTokenProvider&lt;TUser&gt;</c> under <see cref="TokenOptions.DefaultPhoneProvider"/>.
/// </summary>
public class AbpPhoneNumberTwoFactorTokenProvider : AbpTwoFactorTokenProvider
{
    public const string ProviderName = "AbpPhoneNumberTwoFactor";

    public override string Name => ProviderName;

    public AbpPhoneNumberTwoFactorTokenProvider(
        IOptions<AbpPhoneNumberTwoFactorTokenProviderOptions> options,
        IIdentityUserRepository userRepository,
        ICancellationTokenProvider cancellationTokenProvider,
        IClock clock,
        IDataProtectionProvider dataProtectionProvider)
        : base(options.Value, userRepository, cancellationTokenProvider, clock, dataProtectionProvider)
    {
    }

    public override async Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<IdentityUser> manager, IdentityUser user)
    {
        var phoneNumber = await manager.GetPhoneNumberAsync(user);
        return !string.IsNullOrWhiteSpace(phoneNumber) && await manager.IsPhoneNumberConfirmedAsync(user);
    }
}
