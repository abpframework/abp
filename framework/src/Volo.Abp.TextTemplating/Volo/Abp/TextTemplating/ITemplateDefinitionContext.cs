using System.Collections.Generic;

namespace Volo.Abp.TextTemplating
{
    public interface ITemplateDefinitionContext
    {
        IReadOnlyList<TemplateDefinition> GetAll(string name);

        TemplateDefinition GetOrNull(string name);

        void Add(params TemplateDefinition[] definitions);
    }
}