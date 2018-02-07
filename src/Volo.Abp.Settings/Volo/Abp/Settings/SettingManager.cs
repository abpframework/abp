using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        protected Lazy<List<ISettingValueProvider>> Providers { get; }

        protected SettingOptions Options { get; }

        protected ISettingStore SettingStore { get; }

        public SettingManager(
            IOptions<SettingOptions> options,
            IServiceProvider serviceProvider,
            ISettingDefinitionManager settingDefinitionManager,
            ISettingStore settingStore)
        {
            SettingStore = settingStore;
            SettingDefinitionManager = settingDefinitionManager;
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
            Check.NotNull(name, nameof(name));

            var setting = SettingDefinitionManager.Get(name);
            var contributors = Enumerable.Reverse(Providers.Value);

            foreach (var contributor in contributors)
            {
                var value = await contributor.GetOrNullAsync(setting, null);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }

        public virtual async Task<string> GetOrNullAsync(string name, string entityType, string entityId, bool fallback = true)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(entityType, nameof(entityType));

            var setting = SettingDefinitionManager.Get(name);
            var providers = Enumerable
                .Reverse(Providers.Value)
                .SkipWhile(c => c.EntityType != entityType);

            if (!fallback)
            {
                providers = providers.TakeWhile(c => c.EntityType == entityType);
            }

            foreach (var provider in providers)
            {
                var value = await provider.GetOrNullAsync(setting, entityId);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
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
                        settingValues[setting.Name] = new SettingValue(setting.Name, value);
                    }
                }
            }

            return settingValues.Values.ToList();
        }

        public virtual async Task<List<SettingValue>> GetAllAsync(string entityType, string entityId, bool fallback = true)
        {
            Check.NotNull(entityType, nameof(entityType));

            var settingValues = new Dictionary<string, SettingValue>();
            var settingDefinitions = SettingDefinitionManager.GetAll();
            var providers = Enumerable.Reverse(Providers.Value)
                .SkipWhile(c => c.EntityType != entityType);

            if (!fallback)
            {
                providers = providers.TakeWhile(c => c.EntityType == entityType);
            }

            providers = providers.Reverse();

            foreach (var provider in providers)
            {
                foreach (var setting in settingDefinitions)
                {
                    var value = await provider.GetOrNullAsync(setting, entityId);
                    if (value != null)
                    {
                        settingValues[setting.Name] = new SettingValue(setting.Name, value);
                    }
                }
            }

            return settingValues.Values.ToList();
        }

        public virtual Task SetAsync(string name, string value, bool forceToSet = false)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task SetAsync(string name, string value, string entityType, string entityId, bool forceToSet = false)
        {
            throw new System.NotImplementedException();
        }
    }
}