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
            if (setting.IsEncrypted)
            {
                value = SettingEncryptionService.Decrypt(setting, value);
            }

            return value;
        }

        public virtual async Task<List<SettingValue>> GetAllAsync()
        {
            var settingValues = new Dictionary<string, SettingValue>();
            var settingDefinitions = SettingDefinitionManager.GetAll();

            foreach (var provider in SettingValueProviderManager.Providers)
            {
                foreach (var setting in settingDefinitions)
                {
                    var value = await provider.GetOrNullAsync(setting);
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