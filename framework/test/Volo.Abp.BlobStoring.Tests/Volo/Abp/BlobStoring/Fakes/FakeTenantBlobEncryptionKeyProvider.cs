using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// A custom key provider giving each tenant its own passphrase.
/// </summary>
public class FakeTenantBlobEncryptionKeyProvider : DefaultBlobEncryptionKeyProvider
{
    public const string PassPhrasePrefix = "tenant-passphrase-";

    protected ICurrentTenant CurrentTenant { get; }

    public FakeTenantBlobEncryptionKeyProvider(
        ICurrentTenant currentTenant,
        IOptions<AbpBlobStoringEncryptionOptions> options)
        : base(options)
    {
        CurrentTenant = currentTenant;
    }

    public override Task<BlobEncryptionKey> ResolveForEncryptionAsync(
        BlobContainerConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        var containerPassPhrase = GetContainerPassPhraseOrNull(configuration);
        if (string.IsNullOrWhiteSpace(containerPassPhrase) && CurrentTenant.Id.HasValue)
        {
            return Task.FromResult(new BlobEncryptionKey(
                BlobEncryptionKeySource.Tenant,
                GetPassPhrase(CurrentTenant.Id.Value)
            ));
        }

        return base.ResolveForEncryptionAsync(configuration, cancellationToken);
    }

    public override Task<string> ResolveForDecryptionAsync(
        BlobEncryptionKeySource keySource,
        BlobContainerConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        if (keySource == BlobEncryptionKeySource.Tenant)
        {
            if (!CurrentTenant.Id.HasValue)
            {
                throw new AbpException("The BLOB was encrypted with a tenant-specific passphrase, but there is no current tenant!");
            }

            return Task.FromResult(GetPassPhrase(CurrentTenant.Id.Value));
        }

        return base.ResolveForDecryptionAsync(keySource, configuration, cancellationToken);
    }

    public static string GetPassPhrase(Guid tenantId)
    {
        return PassPhrasePrefix + tenantId.ToString("N");
    }
}
