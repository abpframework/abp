using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// A custom key provider giving each tenant its own passphrase, selected from the
/// tenant carried by the <see cref="BlobEncryptionKeyContext"/>.
/// </summary>
public class FakeTenantBlobEncryptionKeyProvider : DefaultBlobEncryptionKeyProvider
{
    public const string PassPhrasePrefix = "tenant-passphrase-";

    public FakeTenantBlobEncryptionKeyProvider(
        IOptions<AbpBlobStoringEncryptionOptions> options)
        : base(options)
    {
    }

    public override Task<BlobEncryptionKey> ResolveForEncryptionAsync(
        BlobEncryptionKeyContext context,
        CancellationToken cancellationToken = default)
    {
        var containerPassPhrase = GetContainerPassPhraseOrNull(context.Configuration);
        if (string.IsNullOrWhiteSpace(containerPassPhrase) && context.TenantId.HasValue)
        {
            return Task.FromResult(new BlobEncryptionKey(
                BlobEncryptionKeySource.Tenant,
                GetPassPhrase(context.TenantId.Value)
            ));
        }

        return base.ResolveForEncryptionAsync(context, cancellationToken);
    }

    public override Task<string> ResolveForDecryptionAsync(
        BlobEncryptionKeySource keySource,
        BlobEncryptionKeyContext context,
        CancellationToken cancellationToken = default)
    {
        if (keySource == BlobEncryptionKeySource.Tenant)
        {
            if (!context.TenantId.HasValue)
            {
                throw new AbpException("The BLOB was encrypted with a tenant-specific passphrase, but there is no current tenant!");
            }

            return Task.FromResult(GetPassPhrase(context.TenantId.Value));
        }

        return base.ResolveForDecryptionAsync(keySource, context, cancellationToken);
    }

    public static string GetPassPhrase(Guid tenantId)
    {
        return PassPhrasePrefix + tenantId.ToString("N");
    }
}
