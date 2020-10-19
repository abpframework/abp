using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.IdentityServer.Grants
{
    public interface IPersistentGrantRepository : IBasicRepository<PersistedGrant, Guid>
    {
        Task<List<PersistedGrant>> GetListAsync(
            string subjectId,
            string sessionId,
            string clientId,
            string type, bool includeDetails = false, CancellationToken cancellationToken = default);

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
            string subjectId = null,
            string sessionId = null,
            string clientId = null,
            string type = null,
            CancellationToken cancellationToken = default
        );
    }
}
