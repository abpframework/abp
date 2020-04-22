using JetBrains.Annotations;
using Volo.Abp.TextTemplating.VirtualFiles;

namespace Volo.Abp.TextTemplating
{
    public static class TemplateDefinitionExtensions
    {
        public static TemplateDefinition AddVirtualFiles(
            [NotNull] this TemplateDefinition templateDefinition,
            [NotNull] string virtualPath)
        {
            Check.NotNull(templateDefinition, nameof(templateDefinition));

            return templateDefinition.AddContributor(
                new VirtualFileTemplateContributor(virtualPath)
            );
        }
    }
}
