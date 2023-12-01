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

    Task<List<IdentitySession>> GetListAsync(Guid userId, CancellationToken cancellationToken = default);

    Task DeleteAllAsync(Guid userId, Guid? exceptSessionId = null, CancellationToken cancellationToken = default);

    Task DeleteAllAsync(Guid userId, string device, Guid? exceptSessionId = null, CancellationToken cancellationToken = default);
}
