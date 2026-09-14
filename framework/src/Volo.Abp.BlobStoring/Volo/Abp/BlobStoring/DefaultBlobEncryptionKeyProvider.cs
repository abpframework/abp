using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// Resolves the container passphrase first, then the global
/// <see cref="AbpBlobStoringEncryptionOptions.DefaultPassPhrase"/>; decryption uses
/// only the source recorded in the BLOB header. Replace this service for
/// tenant-specific or externally stored passphrases.
/// </summary>
public class DefaultBlobEncryptionKeyProvider : IBlobEncryptionKeyProvider, ITransientDependency
{
    protected AbpBlobStoringEncryptionOptions Options { get; }

    public DefaultBlobEncryptionKeyProvider(IOptions<AbpBlobStoringEncryptionOptions> options)
    {
        Options = options.Value;
    }

    /// <inheritdoc />
    public virtual Task<BlobEncryptionKey> ResolveForEncryptionAsync(
        [NotNull] BlobEncryptionKeyContext context,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(context, nameof(context));
        cancellationToken.ThrowIfCancellationRequested();

        var containerPassPhrase = GetContainerPassPhraseOrNull(context.Configuration);
        if (!string.IsNullOrWhiteSpace(containerPassPhrase))
        {
            return Task.FromResult(new BlobEncryptionKey(BlobEncryptionKeySource.Container, containerPassPhrase!));
        }

        if (!string.IsNullOrWhiteSpace(Options.DefaultPassPhrase))
        {
            return Task.FromResult(new BlobEncryptionKey(BlobEncryptionKeySource.Global, Options.DefaultPassPhrase!));
        }

        throw new AbpException(
            "BLOB encryption is enabled, but no passphrase could be resolved. " +
            "Pass a passphrase to the UseEncryption extension method or configure " +
            $"{nameof(AbpBlobStoringEncryptionOptions)}.{nameof(AbpBlobStoringEncryptionOptions.DefaultPassPhrase)}."
        );
    }

    /// <inheritdoc />
    public virtual Task<string> ResolveForDecryptionAsync(
        BlobEncryptionKeySource keySource,
        [NotNull] BlobEncryptionKeyContext context,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(context, nameof(context));
        cancellationToken.ThrowIfCancellationRequested();

        string? passPhrase;
        switch (keySource)
        {
            case BlobEncryptionKeySource.Container:
                passPhrase = GetContainerPassPhraseOrNull(context.Configuration);
                break;
            case BlobEncryptionKeySource.Tenant:
                throw new AbpException(
                    "The BLOB was encrypted with a tenant-specific passphrase, but the default " +
                    $"key provider does not supply tenant keys. Replace the {nameof(IBlobEncryptionKeyProvider)} " +
                    "service with the implementation that was used to encrypt the BLOB."
                );
            case BlobEncryptionKeySource.Global:
                passPhrase = Options.DefaultPassPhrase;
                break;
            default:
                throw new AbpException($"Unknown BLOB encryption key source: {keySource}!");
        }

        if (string.IsNullOrWhiteSpace(passPhrase))
        {
            throw new AbpException(
                $"The BLOB was encrypted with the '{keySource}' passphrase, " +
                "but that passphrase is not available anymore, so the BLOB can not be decrypted."
            );
        }

        return Task.FromResult(passPhrase!);
    }

    /// <summary>
    /// Returns the container-specific passphrase, so derived providers can keep it
    /// as the highest-priority source.
    /// </summary>
    protected virtual string? GetContainerPassPhraseOrNull(BlobContainerConfiguration configuration)
    {
        return BlobEncryptionConfiguration.GetPassPhraseOrNull(configuration);
    }
}
