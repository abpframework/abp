using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Identity.EntityFrameworkCore;

public class EfCoreIdentitySessionRepository : EfCoreRepository<IIdentityDbContext, IdentitySession, Guid>, IIdentitySessionRepository
{
    public EfCoreIdentitySessionRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<IdentitySession> FindAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).FirstOrDefaultAsync(x => x.SessionId == sessionId, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<IdentitySession> GetAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var session = await FindAsync(sessionId, cancellationToken);
        if (session == null)
        {
            throw new EntityNotFoundException(typeof(IdentitySession));
        }

        return session;
    }

    public virtual async Task<List<IdentitySession>> GetListAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Where(x => x.UserId == userId).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async  Task DeleteAllAsync(Guid userId, Guid? exceptSessionId = null, CancellationToken cancellationToken = default)
    {
        await DeleteDirectAsync(x => x.UserId == userId && x.Id != exceptSessionId, cancellationToken);
    }

    public virtual async Task DeleteAllAsync(Guid userId, string device, Guid? exceptSessionId = null, CancellationToken cancellationToken = default)
    {
        await DeleteDirectAsync(x => x.UserId == userId && x.Device == device && x.Id != exceptSessionId, cancellationToken);
    }
}
