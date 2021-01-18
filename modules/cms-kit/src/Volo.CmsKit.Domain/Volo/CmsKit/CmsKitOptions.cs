using JetBrains.Annotations;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit
{
    public class CmsKitOptions
    {
        [NotNull]
        public ReactionDefinitionDictionary Reactions { get; } = new ReactionDefinitionDictionary();
    }
}
