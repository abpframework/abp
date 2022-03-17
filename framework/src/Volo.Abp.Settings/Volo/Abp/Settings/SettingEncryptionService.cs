using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Encryption;

namespace Volo.Abp.Settings;

public class SettingEncryptionService : ISettingEncryptionService, ITransientDependency
{
    protected IStringEncryptionService StringEncryptionService { get; }
    public ILogger<SettingEncryptionService> Logger { get; set; }

    public SettingEncryptionService(IStringEncryptionService stringEncryptionService)
    {
        StringEncryptionService = stringEncryptionService;
        Logger = NullLogger<SettingEncryptionService>.Instance;
    }

    public virtual string Encrypt(SettingDefinition settingDefinition, string plainValue)
    {
        if (plainValue.IsNullOrEmpty())
        {
            return plainValue;
        }

        return StringEncryptionService.Encrypt(plainValue);
    }

    public virtual string Decrypt(SettingDefinition settingDefinition, string encryptedValue)
    {
        if (encryptedValue.IsNullOrEmpty())
        {
            return encryptedValue;
        }

        try
        {
            return StringEncryptionService.Decrypt(encryptedValue);
        }
        catch (Exception e)
        {
            Logger.LogException(e);
            return string.Empty;
        }
    }
}
