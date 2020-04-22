using System.Collections.Generic;
using System.Collections.Immutable;

namespace Volo.Abp.TextTemplating
{
    public class TemplateDefinitionContext : ITemplateDefinitionContext
    {
        protected Dictionary<string, TemplateDefinition> EmailTemplates { get; }

        public TemplateDefinitionContext(Dictionary<string, TemplateDefinition> emailTemplates)
        {
            EmailTemplates = emailTemplates;
        }

        public virtual TemplateDefinition GetOrNull(string name)
        {
            return EmailTemplates.GetOrDefault(name);
        }

        public virtual IReadOnlyList<TemplateDefinition> GetAll()
        {
            return EmailTemplates.Values.ToImmutableList();
        }

        public virtual void Add(params TemplateDefinition[] definitions)
        {
            if (definitions.IsNullOrEmpty())
            {
                return;
            }

            foreach (var definition in definitions)
            {
                EmailTemplates[definition.Name] = definition;
            }
        }
    }
}