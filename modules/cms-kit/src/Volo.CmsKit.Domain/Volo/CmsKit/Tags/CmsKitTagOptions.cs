using JetBrains.Annotations;

namespace Volo.CmsKit.Tags
{
    public class CmsKitTagOptions
    {
        [NotNull]
        public TagEntityTypeDefinitionDictionary EntityTypes { get; } = new TagEntityTypeDefinitionDictionary();
    }
}
