using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity.AspNetCore;

/// <summary>
/// Base class for ABP token providers that enforce a "single active token" policy: generating a token
/// invalidates the one before it for the same user, provider and purpose.
/// <para>
/// Re-implements ASP.NET Core's <c>DataProtectorTokenProvider</c>, which is only available through the
/// ASP.NET Core shared framework, and stays byte compatible with it.
/// </para>
/// </summary>
public abstract class AbpSingleActiveTokenProvider : IUserTwoFactorTokenProvider<IdentityUser>
{
    /// <summary>
    /// The internal login provider name used to store token hashes among the user's tokens.
    /// Using a bracketed name clearly distinguishes these internal entries from real external
    /// login providers (e.g. Google, GitHub) stored in the same table.
    /// </summary>
    public const string InternalLoginProvider = "[AbpSingleActiveToken]";

    protected static readonly Encoding PayloadEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

    protected AbpDataProtectionTokenProviderOptions Options { get; }

    protected IDataProtector Protector { get; }

    protected ILogger<AbpSingleActiveTokenProvider> Logger { get; }

    protected IIdentityUserRepository UserRepository { get; }

    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    public string Name => Options.Name;

    protected AbpSingleActiveTokenProvider(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<AbpDataProtectionTokenProviderOptions> options,
        ILogger<AbpSingleActiveTokenProvider> logger,
        IIdentityUserRepository userRepository,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        Check.NotNull(dataProtectionProvider, nameof(dataProtectionProvider));
        Check.NotNull(options, nameof(options));

        Options = options.Value;
        Protector = dataProtectionProvider.CreateProtector(Options.Name ?? "DataProtectorTokenProvider");
        Logger = logger ?? NullLogger<AbpSingleActiveTokenProvider>.Instance;
        UserRepository = userRepository;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    public virtual async Task<string> GenerateAsync(string purpose, UserManager<IdentityUser> manager, IdentityUser user)
    {
        Check.NotNull(user, nameof(user));

        var token = await ProtectAsync(purpose, manager, user);

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Tokens, CancellationTokenProvider.Token);
        var tokenHash = ComputeSha256Hash(token);
        user.SetToken(InternalLoginProvider, Options.Name + ":" + purpose, tokenHash);

        (await manager.UpdateAsync(user)).CheckErrors();

        return token;
    }

    public virtual async Task<bool> ValidateAsync(string purpose, string token, UserManager<IdentityUser> manager, IdentityUser user)
    {
        if (!await UnprotectAsync(purpose, token, manager, user))
        {
            return false;
        }

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Tokens, CancellationTokenProvider.Token);

        var storedHash = user.FindToken(InternalLoginProvider, Options.Name + ":" + purpose)?.Value;
        if (storedHash == null)
        {
            Logger.LogDebug("No stored hash for the '{ProviderName}' token and the '{Purpose}' purpose. It was never issued, it was removed, or the token came from a provider that does not store one.", Options.Name, purpose);
            return false;
        }

        var inputHash = ComputeSha256Hash(token);
        try
        {
            var storedHashBytes = Convert.FromHexString(storedHash);
            var inputHashBytes = Convert.FromHexString(inputHash);
            return CryptographicOperations.FixedTimeEquals(storedHashBytes, inputHashBytes);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public virtual Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<IdentityUser> manager, IdentityUser user)
    {
        return Task.FromResult(false);
    }

    /// <summary>
    /// Keep in sync with ASP.NET Core's <c>DataProtectorTokenProvider</c>: the two have to stay
    /// interchangeable under the same provider name.
    /// </summary>
    protected virtual async Task<string> ProtectAsync(string purpose, UserManager<IdentityUser> manager, IdentityUser user)
    {
        var stream = new MemoryStream();
        var userId = await manager.GetUserIdAsync(user);

        using (var writer = new BinaryWriter(stream, PayloadEncoding, leaveOpen: true))
        {
            // Not IClock: the payload has to stay byte compatible with ASP.NET Core's.
            writer.Write(DateTimeOffset.UtcNow.UtcTicks);
            writer.Write(userId);
            writer.Write(purpose ?? "");
            writer.Write(manager.SupportsUserSecurityStamp ? await manager.GetSecurityStampAsync(user) ?? "" : "");
        }

        return Convert.ToBase64String(Protector.Protect(stream.ToArray()));
    }

    protected virtual async Task<bool> UnprotectAsync(string purpose, string token, UserManager<IdentityUser> manager, IdentityUser user)
    {
        try
        {
            var stream = new MemoryStream(Protector.Unprotect(Convert.FromBase64String(token)));
            using (var reader = new BinaryReader(stream, PayloadEncoding, leaveOpen: true))
            {
                var creationTime = new DateTimeOffset(reader.ReadInt64(), TimeSpan.Zero);
                if (creationTime + Options.TokenLifespan < DateTimeOffset.UtcNow)
                {
                    Logger.LogDebug("Invalid expiration time for the '{ProviderName}' token.", Options.Name);
                    return false;
                }

                if (reader.ReadString() != await manager.GetUserIdAsync(user))
                {
                    Logger.LogDebug("User ID of the '{ProviderName}' token does not match the current user.", Options.Name);
                    return false;
                }

                var tokenPurpose = reader.ReadString();
                if (!string.Equals(tokenPurpose, purpose))
                {
                    Logger.LogDebug("Purpose of the '{ProviderName}' token is '{TokenPurpose}' but '{Purpose}' was expected.", Options.Name, tokenPurpose, purpose);
                    return false;
                }

                var stamp = reader.ReadString();
                if (reader.PeekChar() != -1)
                {
                    Logger.LogDebug("Unexpected data after the end of the '{ProviderName}' token payload.", Options.Name);
                    return false;
                }

                if (manager.SupportsUserSecurityStamp)
                {
                    if (stamp == await manager.GetSecurityStampAsync(user))
                    {
                        return true;
                    }

                    Logger.LogDebug("Security stamp of the '{ProviderName}' token does not match the current one.", Options.Name);
                    return false;
                }

                if (stamp == "")
                {
                    return true;
                }

                Logger.LogDebug("Security stamp of the '{ProviderName}' token is not empty.", Options.Name);
                return false;
            }
        }
        catch (Exception)
        {
            // Without the exception, the way ASP.NET Core's provider logs this: it carries the key id and
            // the key ring location.
            Logger.LogDebug("Could not read the '{ProviderName}' token. It was protected under another provider name, key ring or application name, or the payload is not a token this provider produced.", Options.Name);
            return false;
        }
    }

    protected virtual string ComputeSha256Hash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }
}
