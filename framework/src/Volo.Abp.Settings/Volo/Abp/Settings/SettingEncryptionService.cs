using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Encryption;

namespace Volo.Abp.Settings;

public class SettingEncryptionService : ISettingEncryptionService, ITransientDependency
{
    protected IStringEncryptionService StringEncryptionService { get; }
    public ILogger<SettingEncryptionService> Logger { get; set; }
    protected IOptions<AbpSettingOptions> Options { get; }

    public SettingEncryptionService(IStringEncryptionService stringEncryptionService, IOptions<AbpSettingOptions> options)
    {
        StringEncryptionService = stringEncryptionService;
        Options = options;
        Logger = NullLogger<SettingEncryptionService>.Instance;
    }

    public virtual string? Encrypt(SettingDefinition settingDefinition, string? plainValue)
    {
        if (plainValue.IsNullOrEmpty())
        {
            return plainValue;
        }

        return StringEncryptionService.Encrypt(plainValue);
    }

    public virtual string? Decrypt(SettingDefinition settingDefinition, string? encryptedValue)
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
            if (Options.Value.ReturnOriginalValueIfDecryptFailed)
            {
                Logger.LogWarning(e, "Failed to decrypt the setting: {0}. Returning the original value...", settingDefinition.Name);
                return encryptedValue;
            }
            
            Logger.LogException(e);
            
            return string.Empty;
        }
    }
}
