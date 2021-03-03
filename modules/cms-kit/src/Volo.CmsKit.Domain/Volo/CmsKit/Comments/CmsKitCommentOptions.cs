using JetBrains.Annotations;
using System.Collections.Generic;

namespace Volo.CmsKit.Comments
{
    public class CmsKitCommentOptions
    {
        [NotNull]
        public List<CommentEntityTypeDefinition> EntityTypes { get; } = new List<CommentEntityTypeDefinition>();
    }
}
