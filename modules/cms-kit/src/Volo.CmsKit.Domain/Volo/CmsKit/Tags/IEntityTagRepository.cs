using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Tags;

public interface IEntityTagRepository : IBasicRepository<EntityTag>
{
    Task<EntityTag> FindAsync(
        [NotNull] Guid tagId,
        [NotNull] string entityId,
        [CanBeNull] Guid? tenantId,
        CancellationToken cancellationToken = default);

    Task DeleteManyAsync(Guid[] tagIds, CancellationToken cancellationToken = default);

    Task<List<string>> GetEntityIdsFilteredByTagAsync(
        [NotNull] Guid tagId,
        [CanBeNull] Guid? tenantId,
        CancellationToken cancellationToken = default);

    Task<List<string>> GetEntityIdsFilteredByTagNameAsync(
        [NotNull] string tagName,
        [NotNull] string entityType,
        [CanBeNull] Guid? tenantId=null,
        CancellationToken cancellationToken=default);
}
