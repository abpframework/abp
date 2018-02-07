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

        public async Task<string> GetOrNullAsync(string name)
        {
            Check.NotNull(name, nameof(name));

            var setting = SettingDefinitionManager.Get(name);

            var contributors = Enumerable.Reverse(Contributors.Value);

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

            var contributors = Enumerable
                .Reverse(Contributors.Value)
                .SkipWhile(c => c.EntityType != entityType);

            if (!fallback)
            {
                contributors = contributors.TakeWhile(c => c.EntityType == entityType);
            }

            foreach (var contributor in contributors)
            {
                var value = await contributor.GetOrNullAsync(setting, entityId);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }

        public Task<List<SettingValue>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SettingValue>> GetAllAsync(string entityType, string entityId, bool fallback = true)
        {
            throw new System.NotImplementedException();
        }

        public Task SetAsync(string name, string value, bool forceToSet = false)
        {
            throw new System.NotImplementedException();
        }

        public Task SetAsync(string name, string value, string entityType, string entityId, bool forceToSet = false)
        {
            throw new System.NotImplementedException();
        }
    }
}