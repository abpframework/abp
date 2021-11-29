using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.TextTemplating
{
    public interface ITemplateDefinitionManager
    {
        [NotNull]
        TemplateDefinition Get([NotNull] string name);

        [NotNull]
        IReadOnlyList<TemplateDefinition> GetAll();

        [CanBeNull]
        TemplateDefinition GetOrNull(string name);
    }
}