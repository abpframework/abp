#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// Simulates a tenant-level setting value provider that gives each tenant
/// its own encryption passphrase.
/// </summary>
public class FakeTenantPassPhraseSettingValueProvider : SettingValueProvider
{
    public const string PassPhrasePrefix = "tenant-passphrase-";

    protected ICurrentTenant CurrentTenant { get; }

    protected ISettingEncryptionService SettingEncryptionService { get; }

    public FakeTenantPassPhraseSettingValueProvider(
        ISettingStore settingStore,
        ICurrentTenant currentTenant,
        ISettingEncryptionService settingEncryptionService)
        : base(settingStore)
    {
        CurrentTenant = currentTenant;
        SettingEncryptionService = settingEncryptionService;
    }

    public override string Name => TenantSettingValueProvider.ProviderName;

    public override Task<string?> GetOrNullAsync(SettingDefinition setting)
    {
        if (setting.Name == BlobStoringEncryptionSettings.TenantPassPhrase && CurrentTenant.Id.HasValue)
        {
            return Task.FromResult(
                SettingEncryptionService.Encrypt(setting, GetPassPhrase(CurrentTenant.Id.Value))
            );
        }

        return Task.FromResult<string?>(null);
    }

    public override Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings)
    {
        return Task.FromResult(
            settings
                .Select(s => new SettingValue(s.Name, null))
                .ToList()
        );
    }

    public static string GetPassPhrase(System.Guid tenantId)
    {
        return PassPhrasePrefix + tenantId.ToString("N");
    }
}
