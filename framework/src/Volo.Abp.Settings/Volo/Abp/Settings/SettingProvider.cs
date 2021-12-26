using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings
{
    public class SettingProvider : ISettingProvider, ITransientDependency
    {
        protected ISettingDefinitionManager SettingDefinitionManager { get; }
        protected ISettingEncryptionService SettingEncryptionService { get; }
        protected ISettingValueProviderManager SettingValueProviderManager { get; }

        public SettingProvider(
            ISettingDefinitionManager settingDefinitionManager,
            ISettingEncryptionService settingEncryptionService,
            ISettingValueProviderManager settingValueProviderManager)
        {
            SettingDefinitionManager = settingDefinitionManager;
            SettingEncryptionService = settingEncryptionService;
            SettingValueProviderManager = settingValueProviderManager;
        }

        public virtual async Task<string> GetOrNullAsync(string name)
        {
            var setting = SettingDefinitionManager.Get(name);
            var providers = Enumerable
                .Reverse(SettingValueProviderManager.Providers);

            if (setting.Providers.Any())
            {
                providers = providers.Where(p => setting.Providers.Contains(p.Name));
            }

            //TODO: How to implement setting.IsInherited?

            var value = await GetOrNullValueFromProvidersAsync(providers, setting);
            if (value != null && setting.IsEncrypted)
            {
                value = SettingEncryptionService.Decrypt(setting, value);
            }

            return value;
        }

        public virtual async Task<List<SettingValue>> GetAllAsync(string[] names)
        {
            var result = new Dictionary<string, SettingValue>();
            var settingDefinitions = SettingDefinitionManager.GetAll().Where(x => names.Contains(x.Name)).ToList();

            foreach (var definition in settingDefinitions)
            {
                result.Add(definition.Name, new SettingValue(definition.Name, null));
            }

            foreach (var provider in Enumerable.Reverse(SettingValueProviderManager.Providers))
            {
                var settingValues = await provider.GetAllAsync(settingDefinitions.Where(x => !x.Providers.Any() || x.Providers.Contains(provider.Name)).ToArray());

                var notNullValues = settingValues.Where(x => x.Value != null).ToList();
                foreach (var settingValue in notNullValues)
                {
                    var settingDefinition = settingDefinitions.First(x => x.Name == settingValue.Name);
                    if (settingDefinition.IsEncrypted)
                    {
                        settingValue.Value = SettingEncryptionService.Decrypt(settingDefinition, settingValue.Value);
                    }

                    if (result.ContainsKey(settingValue.Name) && result[settingValue.Name].Value == null)
                    {
                        result[settingValue.Name].Value = settingValue.Value;
                    }
                }

                settingDefinitions.RemoveAll(x => notNullValues.Any(v => v.Name == x.Name));
                if (!settingDefinitions.Any())
                {
                    break;
                }
            }

            return result.Values.ToList();
        }

        public virtual async Task<List<SettingValue>> GetAllAsync()
        {
            var settingValues = new List<SettingValue>();
            var settingDefinitions = SettingDefinitionManager.GetAll();

            foreach (var setting in settingDefinitions)
            {
                settingValues.Add(new SettingValue(setting.Name, await GetOrNullAsync(setting.Name)));
            }

            return settingValues;
        }

        protected virtual async Task<string> GetOrNullValueFromProvidersAsync(
            IEnumerable<ISettingValueProvider> providers,
            SettingDefinition setting)
        {
            foreach (var provider in providers)
            {
                var value = await provider.GetOrNullAsync(setting);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }
    }
}
