using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.BlobStoring.Database
{
    public interface IBlobRepository : IBasicRepository<Blob, Guid>
    {
        Task<Blob> FindAsync(Guid containerId, [NotNull] string name, Guid? tenantId = null, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Guid containerId, [NotNull] string name, Guid? tenantId = null, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(Guid containerId, [NotNull] string name, Guid? tenantId = null, CancellationToken cancellationToken = default);
    }
}