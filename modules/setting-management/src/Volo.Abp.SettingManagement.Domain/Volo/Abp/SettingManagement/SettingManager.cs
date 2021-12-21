using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement;

public class SettingManager : ISettingManager, ISingletonDependency
{
    protected ISettingDefinitionManager SettingDefinitionManager { get; }
    protected ISettingEncryptionService SettingEncryptionService { get; }
    protected List<ISettingManagementProvider> Providers => _lazyProviders.Value;
    protected SettingManagementOptions Options { get; }
    private readonly Lazy<List<ISettingManagementProvider>> _lazyProviders;

    public SettingManager(
        IOptions<SettingManagementOptions> options,
        IServiceProvider serviceProvider,
        ISettingDefinitionManager settingDefinitionManager,
        ISettingEncryptionService settingEncryptionService)
    {
        SettingDefinitionManager = settingDefinitionManager;
        SettingEncryptionService = settingEncryptionService;
        Options = options.Value;

        //TODO: Instead, use IHybridServiceScopeFactory and create a scope..?

        _lazyProviders = new Lazy<List<ISettingManagementProvider>>(
            () => Options
                .Providers
                .Select(c => serviceProvider.GetRequiredService(c) as ISettingManagementProvider)
                .ToList(),
            true
        );
    }

    public virtual Task<string> GetOrNullAsync(string name, string providerName, string providerKey, bool fallback = true)
    {
        Check.NotNull(name, nameof(name));
        Check.NotNull(providerName, nameof(providerName));

        return GetOrNullInternalAsync(name, providerName, providerKey, fallback);
    }

    public virtual async Task<List<SettingValue>> GetAllAsync(string providerName, string providerKey, bool fallback = true)
    {
        Check.NotNull(providerName, nameof(providerName));

        var settingDefinitions = SettingDefinitionManager.GetAll();
        var providers = Enumerable.Reverse(Providers)
            .SkipWhile(c => c.Name != providerName);

        if (!fallback)
        {
            providers = providers.TakeWhile(c => c.Name == providerName);
        }

        var providerList = providers.Reverse().ToList();

        if (!providerList.Any())
        {
            return new List<SettingValue>();
        }

        var settingValues = new Dictionary<string, SettingValue>();

        foreach (var setting in settingDefinitions)
        {
            string value = null;

            if (setting.IsInherited)
            {
                foreach (var provider in providerList)
                {
                    var providerValue = await provider.GetOrNullAsync(
                        setting,
                        provider.Name == providerName ? providerKey : null
                    );
                    if (providerValue != null)
                    {
                        value = providerValue;
                    }
                }
            }
            else
            {
                value = await providerList[0].GetOrNullAsync(
                    setting,
                    providerKey
                );
            }

            if (setting.IsEncrypted)
            {
                value = SettingEncryptionService.Decrypt(setting, value);
            }

            if (value != null)
            {
                settingValues[setting.Name] = new SettingValue(setting.Name, value);
            }
        }

        return settingValues.Values.ToList();
    }

    public virtual async Task SetAsync(string name, string value, string providerName, string providerKey, bool forceToSet = false)
    {
        Check.NotNull(name, nameof(name));
        Check.NotNull(providerName, nameof(providerName));

        var setting = SettingDefinitionManager.Get(name);

        var providers = Enumerable
            .Reverse(Providers)
            .SkipWhile(p => p.Name != providerName)
            .ToList();

        if (!providers.Any())
        {
            return;
        }

        if (setting.IsEncrypted)
        {
            value = SettingEncryptionService.Encrypt(setting, value);
        }

        if (providers.Count > 1 && !forceToSet && setting.IsInherited && value != null)
        {
            var fallbackValue = await GetOrNullInternalAsync(name, providers[1].Name, null);
            if (fallbackValue == value)
            {
                //Clear the value if it's same as it's fallback value
                value = null;
            }
        }

        providers = providers
            .TakeWhile(p => p.Name == providerName)
            .ToList(); //Getting list for case of there are more than one provider with same providerName

        if (value == null)
        {
            foreach (var provider in providers)
            {
                await provider.ClearAsync(setting, providerKey);
            }
        }
        else
        {
            foreach (var provider in providers)
            {
                await provider.SetAsync(setting, value, providerKey);
            }
        }
    }

    protected virtual async Task<string> GetOrNullInternalAsync(string name, string providerName, string providerKey, bool fallback = true)
    {
        var setting = SettingDefinitionManager.Get(name);
        var providers = Enumerable
            .Reverse(Providers);

        if (providerName != null)
        {
            providers = providers.SkipWhile(c => c.Name != providerName);
        }

        if (!fallback || !setting.IsInherited)
        {
            providers = providers.TakeWhile(c => c.Name == providerName);
        }

        string value = null;
        foreach (var provider in providers)
        {
            value = await provider.GetOrNullAsync(
                setting,
                provider.Name == providerName ? providerKey : null
            );

            if (value != null)
            {
                break;
            }
        }

        if (setting.IsEncrypted)
        {
            value = SettingEncryptionService.Decrypt(setting, value);
        }

        return value;
    }
}
