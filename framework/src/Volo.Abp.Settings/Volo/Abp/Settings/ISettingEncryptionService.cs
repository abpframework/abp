using JetBrains.Annotations;

namespace Volo.Abp.Settings;

public interface ISettingEncryptionService
{
    [CanBeNull]
    string Encrypt([NotNull] SettingDefinition settingDefinition, [CanBeNull] string plainValue);

    [CanBeNull]
    string Decrypt([NotNull] SettingDefinition settingDefinition, [CanBeNull] string encryptedValue);
}
