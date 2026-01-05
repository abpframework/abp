using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.StaticDefinitions;

namespace Volo.Abp.TextTemplating;

public class StaticTemplateDefinitionStore : IStaticTemplateDefinitionStore, ISingletonDependency
{
    protected IServiceProvider ServiceProvider { get; }
    protected AbpTextTemplatingOptions Options { get; }
    protected IStaticDefinitionCache<TemplateDefinition, Dictionary<string, TemplateDefinition>> DefinitionCache { get; }

    public StaticTemplateDefinitionStore(
        IServiceProvider serviceProvider,
        IOptions<AbpTextTemplatingOptions> options,
        IStaticDefinitionCache<TemplateDefinition, Dictionary<string, TemplateDefinition>> definitionCache)
    {
        ServiceProvider = serviceProvider;
        Options = options.Value;
        DefinitionCache = definitionCache;
    }

    public virtual async Task<TemplateDefinition> GetAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var template = await GetOrNullAsync(name);

        if (template == null)
        {
            throw new AbpException("Undefined template: " + name);
        }

        return template;
    }

    public virtual async Task<IReadOnlyList<TemplateDefinition>> GetAllAsync()
    {
        var defs = await GetTemplateDefinitionsAsync();
        return defs.Values.ToImmutableList();
    }

    public virtual async Task<TemplateDefinition?> GetOrNullAsync(string name)
    {
        var defs = await GetTemplateDefinitionsAsync();
        return defs.GetOrDefault(name);
    }

    protected virtual async Task<Dictionary<string, TemplateDefinition>> GetTemplateDefinitionsAsync()
    {
        return await DefinitionCache.GetOrCreateAsync(CreateTextTemplateDefinitionsAsync);
    }

    protected virtual Task<Dictionary<string, TemplateDefinition>> CreateTextTemplateDefinitionsAsync()
    {
        var templates = new Dictionary<string, TemplateDefinition>();

        using (var scope = ServiceProvider.CreateScope())
        {
            var providers = Options
                .DefinitionProviders
                .Select(p => (scope.ServiceProvider.GetRequiredService(p) as ITemplateDefinitionProvider)!)
                .ToList();

            var context = new TemplateDefinitionContext(templates);

            foreach (var provider in providers)
            {
                provider.PreDefine(context);
            }

            foreach (var provider in providers)
            {
                provider.Define(context);
            }

            foreach (var provider in providers)
            {
                provider.PostDefine(context);
            }
        }

        return Task.FromResult(templates);
    }
}
