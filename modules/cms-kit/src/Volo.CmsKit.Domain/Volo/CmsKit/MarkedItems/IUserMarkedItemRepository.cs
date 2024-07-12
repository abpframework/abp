using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.MarkedItems;

public interface IUserMarkedItemRepository : IBasicRepository<UserMarkedItem, Guid>
{
    Task<UserMarkedItem> FindAsync(
        Guid userId,
        [NotNull] string entityType,
        [NotNull] string entityId,
        CancellationToken cancellationToken = default
    );

    Task<List<UserMarkedItem>> GetListForUserAsync(
        Guid userId,
        [NotNull] string entityType,
        CancellationToken cancellationToken = default
    );

    Task<List<string>> GetEntityIdsFilteredByUserAsync(
        [NotNull] Guid userId,
        [NotNull] string entityType,
        [CanBeNull] Guid? tenantId = null,
        CancellationToken cancellationToken = default
    );
}