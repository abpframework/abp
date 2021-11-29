using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.BlobStoring.Database
{
    public interface IDatabaseBlobContainerRepository : IBasicRepository<DatabaseBlobContainer, Guid>
    {
        Task<DatabaseBlobContainer> FindAsync([NotNull] string name, CancellationToken cancellationToken = default);
    }
}