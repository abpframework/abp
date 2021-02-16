using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Comments
{
    public interface ICommentRepository : IBasicRepository<Comment, Guid>
    {
        Task<CommentWithAuthorQueryResultItem> GetWithAuthorAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<List<CommentWithAuthorQueryResultItem>> GetListWithAuthorsAsync(
            [NotNull] string entityType,
            [NotNull] string entityId,
            CancellationToken cancellationToken = default
        );

        Task DeleteWithRepliesAsync(
            Comment comment,
            CancellationToken cancellationToken = default
        );
    }
}
