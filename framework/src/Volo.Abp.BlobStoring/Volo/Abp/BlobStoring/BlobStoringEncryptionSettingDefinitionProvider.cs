using Volo.Abp.Settings;

namespace Volo.Abp.BlobStoring;

public class BlobStoringEncryptionSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(
                BlobStoringEncryptionSettings.TenantPassPhrase,
                isEncrypted: true
            ).WithProviders(TenantSettingValueProvider.ProviderName)
        );
    }
}
