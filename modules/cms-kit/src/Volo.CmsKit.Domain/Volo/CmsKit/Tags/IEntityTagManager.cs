using JetBrains.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.CmsKit.Tags
{
    public interface IEntityTagManager
    {
        Task<EntityTag> AddTagToEntityAsync(
            [NotNull] Guid tagId,
            [NotNull] string entityType,
            [NotNull] string entityId,
            [CanBeNull] Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        Task RemoveTagFromEntityAsync(
            [NotNull] Guid tagId,
            [NotNull] string entityType,
            [NotNull] string entityId,
            [CanBeNull] Guid? tenantId = null,
            CancellationToken cancellationToken = default);
    }
}
