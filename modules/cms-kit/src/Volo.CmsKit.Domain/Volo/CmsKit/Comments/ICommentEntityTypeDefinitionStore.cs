using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Comments;

namespace Volo.CmsKit.Comments
{
    public interface ICommentEntityTypeDefinitionStore
    {
        Task<CommentEntityTypeDefinition> GetDefinitionAsync([NotNull] string entityType);

        Task<bool> IsDefinedAsync([NotNull] string entityType);
    }
}
