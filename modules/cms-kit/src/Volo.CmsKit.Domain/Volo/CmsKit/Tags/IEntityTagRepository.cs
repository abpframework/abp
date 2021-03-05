using JetBrains.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Tags
{
    public interface IEntityTagRepository : IBasicRepository<EntityTag>
    {
        Task<EntityTag> FindAsync(
            [NotNull] Guid tagId,
            [NotNull] string entityId,
            [CanBeNull] Guid? tenantId,
            CancellationToken cancellationToken = default);

        Task DeleteManyAsync(Guid[] tagIds, CancellationToken cancellationToken = default);
    }
}