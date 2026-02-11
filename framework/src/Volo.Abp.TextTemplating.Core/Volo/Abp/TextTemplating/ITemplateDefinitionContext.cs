using System.Collections.Generic;

namespace Volo.Abp.TextTemplating;

public interface ITemplateDefinitionContext
{
    IReadOnlyList<TemplateDefinition> GetAll();

    TemplateDefinition? GetOrNull(string name);

    void Add(params TemplateDefinition[] definitions);
}
