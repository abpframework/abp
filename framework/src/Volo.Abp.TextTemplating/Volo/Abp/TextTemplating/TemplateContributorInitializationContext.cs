using System;
using JetBrains.Annotations;

namespace Volo.Abp.TextTemplating
{
    public class TemplateContributorInitializationContext
    {
        [NotNull]
        public TemplateDefinition TemplateDefinition { get; }

        [NotNull]
        public IServiceProvider ServiceProvider { get; }

        public TemplateContributorInitializationContext(
            [NotNull] TemplateDefinition templateDefinition,
            [NotNull] IServiceProvider serviceProvider)
        {
            TemplateDefinition = Check.NotNull(templateDefinition, nameof(templateDefinition));
            ServiceProvider = Check.NotNull(serviceProvider, nameof(serviceProvider));
        }
    }
}