using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings
{
    public class SettingManager : ISettingManager, ISingletonDependency
    {
        protected ISettingDefinitionManager SettingDefinitionManager { get; }
        protected ISettingEncryptionService SettingEncryptionService { get; }
        protected Lazy<List<ISettingValueProvider>> Providers { get; }
        protected SettingOptions Options { get; }

        public SettingManager(
            IOptions<SettingOptions> options,
            IServiceProvider serviceProvider,
            ISettingDefinitionManager settingDefinitionManager,
            ISettingEncryptionService settingEncryptionService)
        {
            SettingDefinitionManager = settingDefinitionManager;
            SettingEncryptionService = settingEncryptionService;
            Options = options.Value;

            Providers = new Lazy<List<ISettingValueProvider>>(
                () => Options
                    .ValueProviders
                    .Select(c => serviceProvider.GetRequiredService(c) as ISettingValueProvider)
                    .ToList(),
                true
            );
        }

        public virtual Task<string> GetOrNullAsync(string name)
        {
            Check.NotNull(name, nameof(name));

            return GetOrNullInternalAsync(name, null, null);
        }

        public virtual Task<string> GetOrNullAsync(string name, string providerName, string providerKey, bool fallback = true)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(providerName, nameof(providerName));

            return GetOrNullInternalAsync(name, providerName, providerKey, fallback);
        }

        public virtual async Task<List<SettingValue>> GetAllAsync()
        {
            var settingValues = new Dictionary<string, SettingValue>();
            var settingDefinitions = SettingDefinitionManager.GetAll();

            foreach (var provider in Providers.Value)
            {
                foreach (var setting in settingDefinitions)
                {
                    var value = await provider.GetOrNullAsync(setting, null);
                    if (value != null)
                    {
                        if (setting.IsEncrypted)
                        {
                            value = SettingEncryptionService.Decrypt(setting, value);
                        }

                        settingValues[setting.Name] = new SettingValue(setting.Name, value);
                    }
                }
            }

            return settingValues.Values.ToList();
        }

        public virtual async Task<List<SettingValue>> GetAllAsync(string providerName, string providerKey, bool fallback = true)
        {
            Check.NotNull(providerName, nameof(providerName));

            var settingDefinitions = SettingDefinitionManager.GetAll();
            var providers = Enumerable.Reverse(Providers.Value)
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
                        var providerValue = await provider.GetOrNullAsync(setting, providerKey);
                        if (providerValue != null)
                        {
                            value = providerValue;
                        }
                    }
                }
                else
                {
                    value = await providerList[0].GetOrNullAsync(setting, providerKey);
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
                .Reverse(Providers.Value)
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
                //Clear the value if it's same as it's fallback value
                var fallbackValue = await GetOrNullInternalAsync(name, providers[1].Name, providerKey);
                if (fallbackValue == value)
                {
                    value = null;
                }
            }

            providers = providers
                .TakeWhile(p => p.Name == providerName)
                .ToList(); //Getting list for case of there are more than one provider with same EntityType

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
                .Reverse(Providers.Value);

            if (providerName != null)
            {
                providers = providers.SkipWhile(c => c.Name != providerName);
            }

            if (providerName == null || !fallback || !setting.IsInherited)
            {
                providers = providers.TakeWhile(c => c.Name == providerName);
            }

            var value = await GetOrNullValueFromProvidersAsync(providerKey, providers, setting);
            if (setting.IsEncrypted)
            {
                value = SettingEncryptionService.Decrypt(setting, value);
            }

            return value;
        }

        protected virtual async Task<string> GetOrNullValueFromProvidersAsync(
            string providerKey,
            IEnumerable<ISettingValueProvider> providers,
            SettingDefinition setting)
        {
            foreach (var provider in providers)
            {
                var value = await provider.GetOrNullAsync(setting, providerKey);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }
    }
}