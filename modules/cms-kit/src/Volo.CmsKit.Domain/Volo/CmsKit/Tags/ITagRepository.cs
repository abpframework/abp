using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Tags
{
    public interface ITagRepository : IBasicRepository<Tag, Guid>
    {
        Task<Tag> GetAsync(
            [NotNull] string entityType,
            [NotNull] string name,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(
            [NotNull] string entityType,
            [NotNull] string name,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        Task<Tag> FindAsync(
            [NotNull] string entityType,
            [NotNull] string name,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        Task<List<Tag>> GetAllRelatedTagsAsync(
            [NotNull] string entityType,
            [NotNull] string entityId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default); 
    }
}
