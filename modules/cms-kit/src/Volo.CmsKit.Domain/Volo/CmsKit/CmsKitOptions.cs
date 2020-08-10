using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.CmsKit.Reactions;

namespace Volo.CmsKit
{
    public class CmsKitOptions
    {
        [NotNull]
        public ReactionDefinitionDictionary Reactions { get; }

        [NotNull]
        public List<string> PublicCommentEntities { get; }

        public CmsKitOptions()
        {
            Reactions = new ReactionDefinitionDictionary();
            PublicCommentEntities = new List<string>();
        }
    }
}
