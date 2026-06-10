using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity.AspNetCore;

/// <summary>
/// Base class for ABP token providers that enforce a "single active token" policy:
/// generating a new token automatically invalidates all previously issued tokens.
/// Token validity is enforced by SecurityStamp verification (via the base class) and
/// by the stored hash, which is overwritten each time a new token is generated.
/// </summary>
public abstract class AbpSingleActiveTokenProvider : DataProtectorTokenProvider<IdentityUser>
{
    /// <summary>
    /// The internal login provider name used to store token hashes in the user token table.
    /// Using a bracketed name clearly distinguishes these internal entries from real external
    /// login providers (e.g. Google, GitHub) stored in the same table.
    /// </summary>
    public const string InternalLoginProvider = "[AbpSingleActiveToken]";

    protected IIdentityUserRepository UserRepository { get; }

    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    protected AbpSingleActiveTokenProvider(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<DataProtectionTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<IdentityUser>> logger,
        IIdentityUserRepository userRepository,
        ICancellationTokenProvider cancellationTokenProvider)
        : base(dataProtectionProvider, options, logger)
    {
        UserRepository = userRepository;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    public override async Task<string> GenerateAsync(string purpose, UserManager<IdentityUser> manager, IdentityUser user)
    {
        var token = await base.GenerateAsync(purpose, manager, user);

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Tokens, CancellationTokenProvider.Token);
        var tokenHash = ComputeSha256Hash(token);
        user.SetToken(InternalLoginProvider, Options.Name + ":" + purpose, tokenHash);

        await manager.UpdateAsync(user);

        return token;
    }

    public override async Task<bool> ValidateAsync(string purpose, string token, UserManager<IdentityUser> manager, IdentityUser user)
    {
        if (!await base.ValidateAsync(purpose, token, manager, user))
        {
            return false;
        }

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Tokens, CancellationTokenProvider.Token);

        var storedHash = user.FindToken(InternalLoginProvider, Options.Name + ":" + purpose)?.Value;
        if (storedHash == null)
        {
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
            // In case the stored hash is corrupted or not a valid hex string,
            // treat the token as invalid rather than throwing.
            return false;
        }
    }

    protected virtual string ComputeSha256Hash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }
}
