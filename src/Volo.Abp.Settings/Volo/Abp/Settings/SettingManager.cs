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

        protected Lazy<List<ISettingContributor>> Contributors { get; }

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

            Contributors = new Lazy<List<ISettingContributor>>(
                () => Options
                    .Contributors
                    .Select(c => serviceProvider.GetRequiredService(c) as ISettingContributor)
                    .ToList(),
                true
            );
        }

        public Task<string> GetOrNullAsync(string name, bool fallback = true)
        {
            return GetOrNullAsync(name, null, null, fallback);
        }

        public async Task<string> GetOrNullAsync(string name, string entityType, string entityId, bool fallback = true)
        {
            var settingDefinition = SettingDefinitionManager.Get(name);

            foreach (var contributor in GetContributors(entityType, fallback))
            {
                var value = await GetContributorValue(contributor, name, entityId, fallback);
                if (value != null)
                {
                    return value;
                }
            }

            var defaultStoreValue = await SettingStore.GetOrNullAsync(name, null, null);
            if (defaultStoreValue != null)
            {
                return defaultStoreValue;
            }

            if (!fallback)
            {
                return null;
            }

            return settingDefinition.DefaultValue;
        }
        
        public Task<List<SettingValue>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SettingValue>> GetAllAsync(string entityType, string entityId, bool fallback = true)
        {
            throw new System.NotImplementedException();
        }

        public Task SetAsync(string name, string value)
        {
            throw new System.NotImplementedException();
        }

        public Task SetAsync(string name, string value, string entityType, string entityId)
        {
            throw new System.NotImplementedException();
        }

        private static async Task<string> GetContributorValue(ISettingContributor contributor, string name, string entityId, bool fallback)
        {
            if (entityId != null)
            {
                return await contributor.GetOrNullAsync(name, entityId, fallback);
            }
            else
            {
                return await contributor.GetOrNullAsync(name, fallback);
            }
        }

        private IEnumerable<ISettingContributor> GetContributors(string entityType, bool fallback)
        {
            var contributors = Enumerable.Reverse(Contributors.Value);

            if (entityType != null)
            {
                contributors = contributors.SkipWhile(c => c.EntityType != entityType);
            }

            if (!fallback)
            {
                contributors = contributors.TakeWhile(c => c.EntityType == entityType);
            }

            return contributors;
        }
    }
}