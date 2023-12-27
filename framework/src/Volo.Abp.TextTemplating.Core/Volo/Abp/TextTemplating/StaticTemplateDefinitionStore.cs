using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.TextTemplating;

public class StaticTemplateDefinitionStore : IStaticTemplateDefinitionStore, ISingletonDependency
{
    protected Lazy<IDictionary<string, TemplateDefinition>> TemplateDefinitions { get; }

    protected AbpTextTemplatingOptions Options { get; }

    protected IServiceProvider ServiceProvider { get; }

    public StaticTemplateDefinitionStore(IOptions<AbpTextTemplatingOptions> options, IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        Options = options.Value;

        TemplateDefinitions = new Lazy<IDictionary<string, TemplateDefinition>>(CreateTextTemplateDefinitions, true);
    }

    public virtual Task<TemplateDefinition> GetAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var template = GetOrNullAsync(name);

        if (template == null)
        {
            throw new AbpException("Undefined template: " + name);
        }

        return template!;
    }

    public virtual Task<IReadOnlyList<TemplateDefinition>> GetAllAsync()
    {
        return Task.FromResult<IReadOnlyList<TemplateDefinition>>(TemplateDefinitions.Value.Values.ToImmutableList());
    }

    public virtual Task<TemplateDefinition?> GetOrNullAsync(string name)
    {
        return Task.FromResult(TemplateDefinitions.Value.GetOrDefault(name));
    }

    protected virtual IDictionary<string, TemplateDefinition> CreateTextTemplateDefinitions()
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

        return templates;
    }
}
