using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB;

public class MongoIdentitySessionRepository : MongoDbRepository<IAbpIdentityMongoDbContext, IdentitySession, Guid>, IIdentitySessionRepository
{
    public MongoIdentitySessionRepository(IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<IdentitySession> FindAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken)))
            .As<IMongoQueryable<IdentitySession>>()
            .FirstOrDefaultAsync(x => x.SessionId == sessionId, GetCancellationToken(cancellationToken));
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
        return await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken)))
            .As<IMongoQueryable<IdentitySession>>()
            .Where(x => x.UserId == userId).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task DeleteAllAsync(Guid userId, Guid? exceptSessionId = null, CancellationToken cancellationToken = default)
    {
        await DeleteDirectAsync(x => x.UserId == userId && x.Id != exceptSessionId, cancellationToken);
    }

    public virtual async Task DeleteAllAsync(Guid userId, string device, Guid? exceptSessionId = null, CancellationToken cancellationToken = default)
    {
        await DeleteDirectAsync(x => x.UserId == userId && x.Device == device && x.Id != exceptSessionId, cancellationToken);
    }
}
