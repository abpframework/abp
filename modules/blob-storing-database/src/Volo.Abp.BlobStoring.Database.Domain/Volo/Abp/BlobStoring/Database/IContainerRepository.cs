using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.BlobStoring.Database
{
    public interface IContainerRepository : IBasicRepository<Container, Guid>
    {
        Task<Container> CreateIfNotExistAsync([NotNull] string name, Guid? tenantId = null, CancellationToken cancellationToken = default);
        
        Task<Container> FindAsync([NotNull] string name, Guid? tenantId = null, CancellationToken cancellationToken = default);
    }
}