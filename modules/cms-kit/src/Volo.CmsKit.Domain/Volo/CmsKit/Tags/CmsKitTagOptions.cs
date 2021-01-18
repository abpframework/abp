using JetBrains.Annotations;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Tags
{
    public class CmsKitTagOptions
    {
        [NotNull]
        public TagEntityTypeDefinitionDictionary EntityTypes { get; } = new TagEntityTypeDefinitionDictionary();
    }
}
