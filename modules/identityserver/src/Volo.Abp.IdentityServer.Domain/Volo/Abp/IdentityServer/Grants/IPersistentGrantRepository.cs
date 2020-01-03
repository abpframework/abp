using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.IdentityServer.Grants
{
    public interface IPersistentGrantRepository : IBasicRepository<PersistedGrant, Guid>
    {
        Task<PersistedGrant> FindByKeyAsync(
            string key,
            CancellationToken cancellationToken = default
        );

        Task<List<PersistedGrant>> GetListBySubjectIdAsync(
            string key,
            CancellationToken cancellationToken = default
        );

        Task<List<PersistedGrant>> GetListByExpirationAsync(
            DateTime maxExpirationDate,
            int maxResultCount,
            CancellationToken cancellationToken = default
        );

        Task DeleteAsync(
            string subjectId,
            string clientId,
            CancellationToken cancellationToken = default
        );

        Task DeleteAsync(
            string subjectId,
            string clientId,
            string type,
            CancellationToken cancellationToken = default
        );
    }
}