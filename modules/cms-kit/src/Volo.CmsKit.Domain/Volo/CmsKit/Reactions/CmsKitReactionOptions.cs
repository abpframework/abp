using JetBrains.Annotations;

namespace Volo.CmsKit.Reactions
{
    public class CmsKitReactionOptions
    {
        [NotNull]
        public ReactionDefinitionDictionary Reactions { get; } = new ReactionDefinitionDictionary();
    }
}
