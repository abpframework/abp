using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings
{
    public class SettingDefinitionManager : ISettingDefinitionManager, ISingletonDependency
    {
        protected Lazy<List<ISettingDefinitionProvider>> Providers { get; }

        protected Lazy<IDictionary<string, SettingDefinition>> SettingDefinitions { get; }

        protected SettingOptions Options { get; }

        private readonly IServiceProvider _serviceProvider;

        public SettingDefinitionManager(
            IOptions<SettingOptions> options,
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Options = options.Value;

            Providers = new Lazy<List<ISettingDefinitionProvider>>(CreateSettingProviders, true);
            SettingDefinitions = new Lazy<IDictionary<string, SettingDefinition>>(CreateSettingDefinitions, true);
        }

        public virtual SettingDefinition Get(string name)
        {
            Check.NotNull(name, nameof(name));

            var setting = GetOrNull(name);

            if (setting == null)
            {
                throw new AbpException("Undefined setting: " + name);
            }

            return setting;
        }

        public virtual IReadOnlyList<SettingDefinition> GetAll()
        {
            return SettingDefinitions.Value.Values.ToImmutableList();
        }

        public virtual SettingDefinition GetOrNull(string name)
        {
            return SettingDefinitions.Value.GetOrDefault(name);
        }

        protected virtual List<ISettingDefinitionProvider> CreateSettingProviders()
        {
            return Options
                .DefinitionProviders
                .Select(p => _serviceProvider.GetRequiredService(p) as ISettingDefinitionProvider)
                .ToList();
        }

        protected virtual IDictionary<string, SettingDefinition> CreateSettingDefinitions()
        {
            var settings = new Dictionary<string, SettingDefinition>();

            foreach (var provider in Providers.Value)
            {
                provider.Define(new SettingDefinitionContext(settings));
            }

            return settings;
        }
    }
}