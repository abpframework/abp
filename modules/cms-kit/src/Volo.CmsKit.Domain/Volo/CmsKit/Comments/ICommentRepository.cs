using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Comments
{
    public interface ICommentRepository : IBasicRepository<Comment, Guid>
    {
        Task<List<CommentWithAuthor>> GetListWithAuthorsAsync(
            [NotNull] string entityType,
            [NotNull] string entityId);
    }
}
