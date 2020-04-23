using System.Collections.Generic;
using System.Collections.Immutable;

namespace Volo.Abp.TextTemplating
{
    public class TemplateDefinitionContext : ITemplateDefinitionContext
    {
        protected Dictionary<string, TemplateDefinition> TextTemplates { get; }

        public TemplateDefinitionContext(Dictionary<string, TemplateDefinition> textTemplates)
        {
            TextTemplates = textTemplates;
        }

        public virtual TemplateDefinition GetOrNull(string name)
        {
            return TextTemplates.GetOrDefault(name);
        }

        public virtual IReadOnlyList<TemplateDefinition> GetAll()
        {
            return TextTemplates.Values.ToImmutableList();
        }

        public virtual void Add(params TemplateDefinition[] definitions)
        {
            if (definitions.IsNullOrEmpty())
            {
                return;
            }

            foreach (var definition in definitions)
            {
                TextTemplates[definition.Name] = definition;
            }
        }
    }
}