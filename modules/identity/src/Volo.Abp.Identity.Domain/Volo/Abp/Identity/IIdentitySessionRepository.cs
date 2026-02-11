using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Identity;

public interface IIdentitySessionRepository : IBasicRepository<IdentitySession, Guid>
{
    Task<IdentitySession> FindAsync(string sessionId, CancellationToken cancellationToken = default);

    Task<IdentitySession> GetAsync(string sessionId, CancellationToken cancellationToken = default);

    Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistAsync(string sessionId, CancellationToken cancellationToken = default);

    Task<List<IdentitySession>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? userId = null,
        string device = null,
        string clientId = null,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        Guid? userId = null,
        string device = null,
        string clientId = null,
        CancellationToken cancellationToken = default);

    Task DeleteAllAsync(Guid userId, Guid? exceptSessionId = null, CancellationToken cancellationToken = default);

    Task DeleteAllAsync(Guid userId, string device, Guid? exceptSessionId = null, CancellationToken cancellationToken = default);

    Task DeleteAllAsync(TimeSpan inactiveTimeSpan, CancellationToken cancellationToken = default);
}
