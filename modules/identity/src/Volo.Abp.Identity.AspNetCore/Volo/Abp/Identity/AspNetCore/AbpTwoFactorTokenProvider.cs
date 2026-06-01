using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Volo.Abp.Identity.AspNetCore;

/// <summary>
/// Base class for ABP two-factor verification code providers (e.g. Email, Phone).
/// Generates a numeric OTP, stores it encrypted under <see cref="IDataProtector"/>
/// with an absolute UTC expiration, and removes the stored entry on successful
/// validation (single-use). Expected to be combined with Identity lockout for
/// rate-limiting.
/// </summary>
public abstract class AbpTwoFactorTokenProvider : IUserTwoFactorTokenProvider<IdentityUser>
{
    public const string InternalLoginProvider = "[AbpTwoFactorToken]";

    protected const char StoredValueSeparator = '|';

    protected const string DataProtectionPurposeRoot = "Volo.Abp.Identity.AspNetCore.AbpTwoFactorTokenProvider";

    protected AbpTwoFactorTokenProviderOptions Options { get; }

    protected IIdentityUserRepository UserRepository { get; }

    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    protected IClock Clock { get; }

    protected IDataProtectionProvider DataProtectionProvider { get; }

    /// <summary>Unique provider name; used as the stored-token key prefix and DataProtection purpose segment.</summary>
    public abstract string Name { get; }

    protected AbpTwoFactorTokenProvider(
        AbpTwoFactorTokenProviderOptions options,
        IIdentityUserRepository userRepository,
        ICancellationTokenProvider cancellationTokenProvider,
        IClock clock,
        IDataProtectionProvider dataProtectionProvider)
    {
        Options = options;
        UserRepository = userRepository;
        CancellationTokenProvider = cancellationTokenProvider;
        Clock = clock;
        DataProtectionProvider = dataProtectionProvider;
    }

    public abstract Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<IdentityUser> manager, IdentityUser user);

    public virtual async Task<string> GenerateAsync(string purpose, UserManager<IdentityUser> manager, IdentityUser user)
    {
        var code = GenerateNumericCode(Options.CodeLength);
        var protectedCode = CreateProtector(purpose).Protect(code);
        var expiresAtUnixSeconds = ToUnixSeconds(Clock.Now.Add(Options.TokenLifespan));
        var storedValue = protectedCode + StoredValueSeparator + expiresAtUnixSeconds.ToString(CultureInfo.InvariantCulture);

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Tokens, CancellationTokenProvider.Token);
        user.SetToken(InternalLoginProvider, GetTokenName(purpose), storedValue);

        (await manager.UpdateAsync(user)).CheckErrors();

        return code;
    }

    public virtual async Task<bool> ValidateAsync(string purpose, string token, UserManager<IdentityUser> manager, IdentityUser user)
    {
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Tokens, CancellationTokenProvider.Token);

        var tokenName = GetTokenName(purpose);
        var stored = user.FindToken(InternalLoginProvider, tokenName)?.Value;
        if (stored == null)
        {
            return false;
        }

        if (!TryParseStoredValue(stored, out var protectedCode, out var expiresAtUnixSeconds))
        {
            await TryRemoveStoredTokenAsync(manager, user, tokenName);
            return false;
        }

        if (ToUnixSeconds(Clock.Now) >= expiresAtUnixSeconds)
        {
            await TryRemoveStoredTokenAsync(manager, user, tokenName);
            return false;
        }

        string storedCode;
        try
        {
            storedCode = CreateProtector(purpose).Unprotect(protectedCode);
        }
        catch (CryptographicException)
        {
            await TryRemoveStoredTokenAsync(manager, user, tokenName);
            return false;
        }
        catch (FormatException)
        {
            await TryRemoveStoredTokenAsync(manager, user, tokenName);
            return false;
        }

        var storedBytes = Encoding.UTF8.GetBytes(storedCode);
        var inputBytes = Encoding.UTF8.GetBytes(token);
        if (storedBytes.Length != inputBytes.Length ||
            !CryptographicOperations.FixedTimeEquals(storedBytes, inputBytes))
        {
            // Keep stored entry so the user can retry until expiration. Callers must rate-limit.
            return false;
        }

        // Translate ConcurrencyStamp failure (another request won the consume race) to false,
        // so legitimate concurrent verification doesn't surface as a 500.
        try
        {
            await RemoveStoredTokenAsync(manager, user, tokenName);
        }
        catch (AbpIdentityResultException)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Removes the stored entry. Throws <see cref="AbpIdentityResultException"/> on persistence
    /// failure so the successful-verification path knows the consume actually committed.
    /// Cleanup paths use <see cref="TryRemoveStoredTokenAsync"/> instead.
    /// </summary>
    protected virtual async Task RemoveStoredTokenAsync(UserManager<IdentityUser> manager, IdentityUser user, string tokenName)
    {
        user.RemoveToken(InternalLoginProvider, tokenName);
        (await manager.UpdateAsync(user)).CheckErrors();
    }

    /// <summary>
    /// Cleanup variant that swallows concurrent-update failures. The next
    /// <see cref="GenerateAsync"/> will overwrite whatever remains.
    /// </summary>
    protected virtual async Task TryRemoveStoredTokenAsync(UserManager<IdentityUser> manager, IdentityUser user, string tokenName)
    {
        try
        {
            await RemoveStoredTokenAsync(manager, user, tokenName);
        }
        catch (AbpIdentityResultException)
        {
        }
    }

    protected virtual string GetTokenName(string purpose)
    {
        return Name + ":" + purpose;
    }

    protected virtual IDataProtector CreateProtector(string purpose)
    {
        return DataProtectionProvider.CreateProtector(DataProtectionPurposeRoot, Name, purpose);
    }

    protected virtual string GenerateNumericCode(int length)
    {
        if (length is <= 0 or > 9)
        {
            // Cap at 9 to stay comfortably within Int32 range.
            throw new ArgumentOutOfRangeException(nameof(length), length, "Code length must be between 1 and 9.");
        }

        var upperBound = (int)Math.Pow(10, length);
        var value = RandomNumberGenerator.GetInt32(0, upperBound);
        return value.ToString(new string('0', length), CultureInfo.InvariantCulture);
    }

    protected virtual long ToUnixSeconds(DateTime moment)
    {
        // Treat Unspecified as local time to match IClock.Now's default behaviour.
        if (moment.Kind == DateTimeKind.Unspecified)
        {
            moment = DateTime.SpecifyKind(moment, DateTimeKind.Local);
        }

        return new DateTimeOffset(moment).ToUnixTimeSeconds();
    }

    private static bool TryParseStoredValue(string stored, out string protectedCode, out long expiresAtUnixSeconds)
    {
        protectedCode = string.Empty;
        expiresAtUnixSeconds = 0;

        var separatorIndex = stored.LastIndexOf(StoredValueSeparator);
        if (separatorIndex <= 0 || separatorIndex == stored.Length - 1)
        {
            return false;
        }

        var protectedPart = stored.Substring(0, separatorIndex);
        var secondsPart = stored.Substring(separatorIndex + 1);

        if (!long.TryParse(secondsPart, NumberStyles.Integer, CultureInfo.InvariantCulture, out var seconds))
        {
            return false;
        }

        protectedCode = protectedPart;
        expiresAtUnixSeconds = seconds;
        return true;
    }
}
