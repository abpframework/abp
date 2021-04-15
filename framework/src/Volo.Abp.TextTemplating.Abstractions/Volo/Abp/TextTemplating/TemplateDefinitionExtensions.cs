using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.TextTemplating.VirtualFiles;

namespace Volo.Abp.TextTemplating
{
    public static class TemplateDefinitionExtensions
    {
        public static TemplateDefinition WithVirtualFilePath(
            [NotNull] this TemplateDefinition templateDefinition,
            [NotNull] string virtualPath,
            bool isInlineLocalized)
        {
            Check.NotNull(templateDefinition, nameof(templateDefinition));

            templateDefinition.IsInlineLocalized = isInlineLocalized;

            return templateDefinition.WithProperty(
                VirtualFileTemplateContentContributor.VirtualPathPropertyName,
                virtualPath
            );
        }

        public static string GetVirtualFilePathOrNull(
            [NotNull] this TemplateDefinition templateDefinition)
        {
            Check.NotNull(templateDefinition, nameof(templateDefinition));

            return templateDefinition
                .Properties
                .GetOrDefault(VirtualFileTemplateContentContributor.VirtualPathPropertyName) as string;
        }
    }
}
