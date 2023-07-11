using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings;

public class StaticSettingDefinitionStore : IStaticSettingDefinitionStore, ISingletonDependency
{
    protected Lazy<IDictionary<string, SettingDefinition>> SettingDefinitions { get; }

    protected AbpSettingOptions Options { get; }

    protected IServiceProvider ServiceProvider { get; }

    public StaticSettingDefinitionStore(IOptions<AbpSettingOptions> options, IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        Options = options.Value;

        SettingDefinitions = new Lazy<IDictionary<string, SettingDefinition>>(CreateSettingDefinitions, true);
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

    public virtual Task<IReadOnlyList<SettingDefinition>> GetAllAsync()
    {
        return Task.FromResult<IReadOnlyList<SettingDefinition>>(SettingDefinitions.Value.Values.ToImmutableList());
    }

    public virtual Task<SettingDefinition?> GetOrNullAsync(string name)
    {
        return Task.FromResult(SettingDefinitions.Value.GetOrDefault(name));
    }

    protected virtual IDictionary<string, SettingDefinition> CreateSettingDefinitions()
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

        return settings;
    }
}
