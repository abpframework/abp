using System;
using JetBrains.Annotations;

namespace Volo.Abp.TextTemplating;

public class TemplateContentContributorContext
{
    [NotNull]
    public TemplateDefinition TemplateDefinition { get; }

    [NotNull]
    public IServiceProvider ServiceProvider { get; }

    public string? Culture { get; }

    public TemplateContentContributorContext(
        [NotNull] TemplateDefinition templateDefinition,
        [NotNull] IServiceProvider serviceProvider,
        string? culture)
    {
        TemplateDefinition = Check.NotNull(templateDefinition, nameof(templateDefinition));
        ServiceProvider = Check.NotNull(serviceProvider, nameof(serviceProvider));
        Culture = culture;
    }
}
