using JetBrains.Annotations;
using Volo.CmsKit.Reactions;

namespace Volo.CmsKit
{
    public class CmsKitOptions
    {
        [NotNull]
        public ReactionDefinitionDictionary Reactions { get; }

        public CmsKitOptions()
        {
            Reactions = new ReactionDefinitionDictionary();
        }
    }
}
