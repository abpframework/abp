using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings
{
    public class SettingProvider : ISettingProvider, ITransientDependency
    {
        protected ISettingDefinitionManager SettingDefinitionManager { get; }
        protected Lazy<List<ISettingValueProvider>> Providers { get; }
        protected ISettingEncryptionService SettingEncryptionService { get; }
        protected SettingOptions Options { get; }

        public SettingProvider(
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

        public virtual async Task<string> GetOrNullAsync(string name)
        {
            var setting = SettingDefinitionManager.Get(name);
            var providers = Enumerable
                .Reverse(Providers.Value);

            if (setting.Providers.Any())
            {
                providers = providers.Where(p => setting.Providers.Contains(p.Name));
            }

            //TODO: How to implement setting.IsInherited?

            var value = await GetOrNullValueFromProvidersAsync(null, providers, setting);
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