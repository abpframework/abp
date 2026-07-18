using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// Default implementation of <see cref="IBlobEncryptionKeyProvider"/>.
/// Resolves a tenant-specific passphrase from the setting system
/// (<see cref="BlobStoringEncryptionSettings.TenantPassPhrase"/>) when a tenant is available,
/// otherwise falls back to the global passphrase
/// (<see cref="AbpBlobStoringEncryptionOptions.DefaultPassPhrase"/>).
/// </summary>
public class DefaultBlobEncryptionKeyProvider : IBlobEncryptionKeyProvider, ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; }

    protected ISettingProvider SettingProvider { get; }

    protected AbpBlobStoringEncryptionOptions Options { get; }

    public DefaultBlobEncryptionKeyProvider(
        ICurrentTenant currentTenant,
        ISettingProvider settingProvider,
        IOptions<AbpBlobStoringEncryptionOptions> options)
    {
        CurrentTenant = currentTenant;
        SettingProvider = settingProvider;
        Options = options.Value;
    }

    public virtual async Task<string?> GetPassPhraseOrNullAsync(
        BlobContainerConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        if (CurrentTenant.Id.HasValue)
        {
            var tenantPassPhrase = await SettingProvider.GetOrNullAsync(
                BlobStoringEncryptionSettings.TenantPassPhrase
            );

            if (!tenantPassPhrase.IsNullOrEmpty())
            {
                return tenantPassPhrase;
            }
        }

        return Options.DefaultPassPhrase;
    }
}
