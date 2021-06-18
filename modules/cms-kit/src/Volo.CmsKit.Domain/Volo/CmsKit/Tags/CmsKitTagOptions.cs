using JetBrains.Annotations;

namespace Volo.CmsKit.Tags
{
    public class CmsKitTagOptions
    {
        [NotNull]
        public TagEntityTypeDefinitions EntityTypes { get; } = new TagEntityTypeDefinitions();
    }
}
