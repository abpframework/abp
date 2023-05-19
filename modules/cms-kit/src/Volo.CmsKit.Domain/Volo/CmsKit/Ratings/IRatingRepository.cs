using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Ratings;

public interface IRatingRepository : IBasicRepository<Rating, Guid>
{
    Task<Rating> GetCurrentUserRatingAsync(
        [NotNull] string entityType,
        [NotNull] string entityId,
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task<List<RatingWithStarCountQueryResultItem>> GetGroupedStarCountsAsync(
        [NotNull] string entityType,
        [NotNull] string entityId,
        CancellationToken cancellationToken = default
    );
}
