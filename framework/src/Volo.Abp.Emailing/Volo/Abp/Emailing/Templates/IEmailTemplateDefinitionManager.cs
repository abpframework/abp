using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Emailing.Templates
{
    public interface IEmailTemplateDefinitionManager
    {
        [NotNull]
        EmailTemplateDefinition Get([NotNull] string name);

        IReadOnlyList<EmailTemplateDefinition> GetAll();

        EmailTemplateDefinition GetOrNull(string name);
    }
}