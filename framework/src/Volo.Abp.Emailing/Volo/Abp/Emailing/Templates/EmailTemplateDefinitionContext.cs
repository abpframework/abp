using System.Collections.Generic;
using System.Collections.Immutable;

namespace Volo.Abp.Emailing.Templates
{
    public class EmailTemplateDefinitionContext : IEmailTemplateDefinitionContext
    {
        protected Dictionary<string, EmailTemplateDefinition> EmailTemplates { get; }

        public EmailTemplateDefinitionContext(Dictionary<string, EmailTemplateDefinition> emailTemplates)
        {
            EmailTemplates = emailTemplates;
        }

        public virtual EmailTemplateDefinition GetOrNull(string name)
        {
            return EmailTemplates.GetOrDefault(name);
        }

        public virtual IReadOnlyList<EmailTemplateDefinition> GetAll()
        {
            return EmailTemplates.Values.ToImmutableList();
        }

        public virtual void Add(params EmailTemplateDefinition[] definitions)
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