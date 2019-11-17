using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Emailing.Templates
{
    public class EmailTemplateDefinitionManager : IEmailTemplateDefinitionManager, ISingletonDependency
    {
        protected Lazy<IDictionary<string, EmailTemplateDefinition>> EmailTemplateDefinitions { get; }

        protected AbpEmailTemplateOptions Options { get; }

        protected IServiceProvider ServiceProvider { get; }

        public EmailTemplateDefinitionManager(
            IOptions<AbpEmailTemplateOptions> options,
            IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Options = options.Value;

            EmailTemplateDefinitions =
                new Lazy<IDictionary<string, EmailTemplateDefinition>>(CreateEmailTemplateDefinitions, true);
        }

        public virtual EmailTemplateDefinition Get(string name)
        {
            Check.NotNull(name, nameof(name));

            var template = GetOrNull(name);

            if (template == null)
            {
                throw new AbpException("Undefined template: " + name);
            }

            return template;
        }

        public virtual IReadOnlyList<EmailTemplateDefinition> GetAll()
        {
            return EmailTemplateDefinitions.Value.Values.ToImmutableList();
        }

        public virtual EmailTemplateDefinition GetOrNull(string name)
        {
            return EmailTemplateDefinitions.Value.GetOrDefault(name);
        }

        protected virtual IDictionary<string, EmailTemplateDefinition> CreateEmailTemplateDefinitions()
        {
            var templates = new Dictionary<string, EmailTemplateDefinition>();

            using (var scope = ServiceProvider.CreateScope())
            {
                var providers = Options
                    .DefinitionProviders
                    .Select(p => scope.ServiceProvider.GetRequiredService(p) as IEmailTemplateDefinitionProvider)
                    .ToList();

                foreach (var provider in providers)
                {
                    provider.Define(new EmailTemplateDefinitionContext(templates));
                }
            }

            return templates;
        }
    }
}