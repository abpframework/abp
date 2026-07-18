using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// A pipeline contributor that encrypts BLOBs on save and decrypts them on read.
/// <para>
/// The passphrase is resolved in the following order:
/// 1. Container-specific passphrase (see <c>UseEncryption</c> extension method).
/// 2. <see cref="IBlobEncryptionKeyProvider"/> (tenant-specific setting, then the global passphrase).
/// </para>
/// </summary>
public class BlobEncryptionContributor : IBlobPipelineContributor, ITransientDependency
{
    protected IBlobEncryptionService EncryptionService { get; }

    protected IBlobEncryptionKeyProvider EncryptionKeyProvider { get; }

    public BlobEncryptionContributor(
        IBlobEncryptionService encryptionService,
        IBlobEncryptionKeyProvider encryptionKeyProvider)
    {
        EncryptionService = encryptionService;
        EncryptionKeyProvider = encryptionKeyProvider;
    }

    public virtual async Task<Stream> OnSaveAsync(BlobPipelineSaveArgs args)
    {
        var passPhrase = await GetPassPhraseAsync(args);
        return EncryptionService.Encrypt(args.BlobStream, passPhrase);
    }

    public virtual async Task<Stream> OnGetAsync(BlobPipelineGetArgs args)
    {
        var passPhrase = await GetPassPhraseAsync(args);
        return EncryptionService.Decrypt(args.BlobStream, passPhrase);
    }

    protected virtual async Task<string> GetPassPhraseAsync(BlobProviderArgs args)
    {
        var passPhrase =
            args.Configuration.GetConfigurationOrDefault<string>(BlobStoringEncryptionConfigurationNames.PassPhrase) ??
            await EncryptionKeyProvider.GetPassPhraseOrNullAsync(args.Configuration, args.CancellationToken);

        if (passPhrase.IsNullOrEmpty())
        {
            throw new AbpException(
                $"BLOB encryption is enabled for the container '{args.ContainerName}', but no passphrase could be resolved. " +
                $"Pass a passphrase to the UseEncryption extension method, set the '{BlobStoringEncryptionSettings.TenantPassPhrase}' " +
                $"setting for the current tenant or configure {nameof(AbpBlobStoringEncryptionOptions)}.{nameof(AbpBlobStoringEncryptionOptions.DefaultPassPhrase)}."
            );
        }

        return passPhrase!;
    }
}
