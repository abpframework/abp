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
using Volo.Abp.Timing;

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

    public virtual async Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(x => x.Id == id, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<bool> ExistAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(x => x.SessionId == sessionId, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentitySession>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? userId = null,
        string device = null,
        string clientId = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(userId.HasValue, x => x.UserId == userId)
            .WhereIf(!device.IsNullOrWhiteSpace(), x => x.Device == device)
            .WhereIf(!clientId.IsNullOrWhiteSpace(), x => x.ClientId == clientId)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(IdentitySession.LastAccessed)} desc" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetCountAsync(
        Guid? userId = null,
        string device = null,
        string clientId = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(userId.HasValue, x => x.UserId == userId)
            .WhereIf(!device.IsNullOrWhiteSpace(), x => x.Device == device)
            .WhereIf(!clientId.IsNullOrWhiteSpace(), x => x.ClientId == clientId)
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async  Task DeleteAllAsync(Guid userId, Guid? exceptSessionId = null, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(x => x.UserId == userId && x.Id != exceptSessionId, cancellationToken: cancellationToken);
    }

    public virtual async Task DeleteAllAsync(Guid userId, string device, Guid? exceptSessionId = null, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(x => x.UserId == userId && x.Device == device && x.Id != exceptSessionId, cancellationToken: cancellationToken);
    }

    public virtual async Task DeleteAllAsync(TimeSpan inactiveTimeSpan, CancellationToken cancellationToken = default)
    {
        var now = LazyServiceProvider.LazyGetRequiredService<IClock>().Now;
        await DeleteDirectAsync(x => x.LastAccessed == null || x.LastAccessed < now.Subtract(inactiveTimeSpan), cancellationToken: cancellationToken);
    }
}
