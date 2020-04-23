using JetBrains.Annotations;
using Volo.Abp.TextTemplating.VirtualFiles;

namespace Volo.Abp.TextTemplating
{
    public static class TemplateDefinitionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateDefinition"></param>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public static TemplateDefinition WithVirtualFilePath(
            [NotNull] this TemplateDefinition templateDefinition,
            [NotNull] string virtualPath)
        {
            Check.NotNull(templateDefinition, nameof(templateDefinition));

            return templateDefinition.WithContributor(
                new VirtualFileTemplateContributor(virtualPath)
            );
        }
    }
}
