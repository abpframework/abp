using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.TextTemplating
{
    public interface ITemplateDefinitionManager
    {
        [NotNull]
        TemplateDefinition Get([NotNull] string name);

        IReadOnlyList<TemplateDefinition> GetAll();

        TemplateDefinition GetOrNull(string name);
    }
}