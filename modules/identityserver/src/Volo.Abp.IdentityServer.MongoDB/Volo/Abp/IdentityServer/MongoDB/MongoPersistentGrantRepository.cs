using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.MongoDB;


namespace Volo.Abp.IdentityServer.MongoDB;

public class MongoPersistentGrantRepository : MongoDbRepository<IAbpIdentityServerMongoDbContext, PersistedGrant, Guid>, IPersistentGrantRepository
{
    public MongoPersistentGrantRepository(IMongoDbContextProvider<IAbpIdentityServerMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<List<PersistedGrant>> GetListAsync(string subjectId, string sessionId, string clientId, string type, bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await FilterAsync(subjectId, sessionId, clientId, type, cancellationToken))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<PersistedGrant> FindByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .Where(x => x.Key == key)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<PersistedGrant>> GetListBySubjectIdAsync(string subjectId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .Where(x => x.SubjectId == subjectId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<PersistedGrant>> GetListByExpirationAsync(DateTime maxExpirationDate, int maxResultCount,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .Where(x => x.Expiration != null && x.Expiration < maxExpirationDate)
            .OrderBy(x => x.ClientId)
            .Take(maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task DeleteExpirationAsync(DateTime maxExpirationDate, CancellationToken cancellationToken = default)
    {
        await DeleteDirectAsync(x => x.Expiration != null && x.Expiration < maxExpirationDate, cancellationToken: cancellationToken);
    }

    public virtual async Task DeleteAsync(
        string subjectId = null,
        string sessionId = null,
        string clientId = null,
        string type = null,
        CancellationToken cancellationToken = default)
    {
        var persistedGrants = await (await FilterAsync(subjectId, sessionId, clientId, type, cancellationToken))
            .ToListAsync(GetCancellationToken(cancellationToken));

        foreach (var persistedGrant in persistedGrants)
        {
            await DeleteAsync(persistedGrant, false, GetCancellationToken(cancellationToken));
        }
    }

    public virtual async Task DeleteAsync(string subjectId, string clientId, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(
            x => x.SubjectId == subjectId && x.ClientId == clientId,
            cancellationToken: GetCancellationToken(cancellationToken)
        );
    }

    public virtual async Task DeleteAsync(string subjectId, string clientId, string type, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(
            x => x.SubjectId == subjectId && x.ClientId == clientId && x.Type == type,
            cancellationToken: GetCancellationToken(cancellationToken)
        );
    }

    private async Task<IQueryable<PersistedGrant>> FilterAsync(
        string subjectId,
        string sessionId,
        string clientId,
        string type,
        CancellationToken cancellationToken = default)
    {
        return (await GetQueryableAsync(cancellationToken))
            .WhereIf(!subjectId.IsNullOrWhiteSpace(), x => x.SubjectId == subjectId)
            .WhereIf(!sessionId.IsNullOrWhiteSpace(), x => x.SessionId == sessionId)
            .WhereIf(!clientId.IsNullOrWhiteSpace(), x => x.ClientId == clientId)
            .WhereIf(!type.IsNullOrWhiteSpace(), x => x.Type == type);
    }
}
