using System.Collections.Generic;
using System.Collections.Immutable;

namespace Volo.Abp.TextTemplating;

public class TemplateDefinitionContext : ITemplateDefinitionContext
{
    protected Dictionary<string, TemplateDefinition> Templates { get; }

    public TemplateDefinitionContext(Dictionary<string, TemplateDefinition> templates)
    {
        Templates = templates;
    }

    public IReadOnlyList<TemplateDefinition> GetAll(string name)
    {
        return Templates.Values.ToImmutableList();
    }

    public virtual TemplateDefinition GetOrNull(string name)
    {
        return Templates.GetOrDefault(name);
    }

    public virtual IReadOnlyList<TemplateDefinition> GetAll()
    {
        return Templates.Values.ToImmutableList();
    }

    public virtual void Add(params TemplateDefinition[] definitions)
    {
        if (definitions.IsNullOrEmpty())
        {
            return;
        }

        foreach (var definition in definitions)
        {
            Templates[definition.Name] = definition;
        }
    }
}
