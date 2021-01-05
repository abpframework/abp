using JetBrains.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Contents
{
    public interface IContentRepository : IBasicRepository<Content, Guid>
    {
        Task<Content> GetAsync(
            [NotNull] string entityType,
            [NotNull] string entityId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);
        
        Task<Content> FindAsync(
            [NotNull] string entityType,
            [NotNull] string entityId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            [NotNull] string entityType,
            [NotNull] string entityId,
            Guid? tenantId = null, 
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            [NotNull] string entityType,
            [NotNull] string entityId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);
    }
}