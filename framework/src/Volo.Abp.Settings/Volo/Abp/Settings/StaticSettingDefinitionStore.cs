using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.StaticDefinitions;

namespace Volo.Abp.Settings;

public class StaticSettingDefinitionStore : IStaticSettingDefinitionStore, ISingletonDependency
{
    protected IServiceProvider ServiceProvider { get; }
    protected AbpSettingOptions Options { get; }
    protected IStaticDefinitionCache<SettingDefinition, Dictionary<string, SettingDefinition>> DefinitionCache { get; }

    public StaticSettingDefinitionStore(
        IServiceProvider serviceProvider,
        IOptions<AbpSettingOptions> options,
        IStaticDefinitionCache<SettingDefinition, Dictionary<string, SettingDefinition>> definitionCache)
    {
        ServiceProvider = serviceProvider;
        Options = options.Value;
        DefinitionCache = definitionCache;
    }

    public virtual async Task<SettingDefinition> GetAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var setting = await GetOrNullAsync(name);

        if (setting == null)
        {
            throw new AbpException("Undefined setting: " + name);
        }

        return setting;
    }

    public virtual async Task<IReadOnlyList<SettingDefinition>> GetAllAsync()
    {
        var defs = await GetSettingDefinitionsAsync();
        return defs.Values.ToImmutableList();
    }

    public virtual async Task<SettingDefinition?> GetOrNullAsync(string name)
    {
        var defs = await GetSettingDefinitionsAsync();
        return defs.GetOrDefault(name);
    }

    protected virtual async Task<Dictionary<string, SettingDefinition>> GetSettingDefinitionsAsync()
    {
        return await DefinitionCache.GetOrCreateAsync(CreateSettingDefinitionsAsync);
    }

    protected virtual Task<Dictionary<string, SettingDefinition>> CreateSettingDefinitionsAsync()
    {
        var settings = new Dictionary<string, SettingDefinition>();

        using (var scope = ServiceProvider.CreateScope())
        {
            var providers = Options
                .DefinitionProviders
                .Select(p => scope.ServiceProvider.GetRequiredService(p) as ISettingDefinitionProvider)
                .ToList();

            foreach (var provider in providers)
            {
                provider?.Define(new SettingDefinitionContext(settings));
            }
        }

        return Task.FromResult(settings);
    }
}
