using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Reactions;

public interface IUserReactionRepository : IBasicRepository<UserReaction, Guid>
{
    Task<UserReaction> FindAsync(
        Guid userId,
        [NotNull] string entityType,
        [NotNull] string entityId,
        [NotNull] string reactionName,
        CancellationToken cancellationToken = default
    );

    Task<List<UserReaction>> GetListForUserAsync(
        Guid userId,
        [NotNull] string entityType,
        [NotNull] string entityId,
        CancellationToken cancellationToken = default
    );

    Task<List<ReactionSummaryQueryResultItem>> GetSummariesAsync(
        [NotNull] string entityType,
        [NotNull] string entityId,
        CancellationToken cancellationToken = default
    );
}
