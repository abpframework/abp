using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.Timing;

namespace Volo.Abp.Identity.MongoDB;

public class MongoIdentityUserDelegationRepository : MongoDbRepository<IAbpIdentityMongoDbContext, IdentityUserDelegation, Guid>, IIdentityUserDelegationRepository
{
    protected IClock Clock { get; }
    
    public MongoIdentityUserDelegationRepository(IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider, IClock clock)
        : base(dbContextProvider)
    {
        Clock = clock;
    }

    public virtual async Task<List<IdentityUserDelegation>> GetListAsync(Guid? sourceUserId, Guid? targetUserId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .WhereIf(sourceUserId.HasValue, x => x.SourceUserId == sourceUserId)
            .WhereIf(targetUserId.HasValue, x => x.TargetUserId == targetUserId)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<List<IdentityUserDelegation>> GetActiveDelegationsAsync(Guid targetUserId, CancellationToken cancellationToken = default)
    {
        var now = Clock.Now;
        return await (await GetQueryableAsync(cancellationToken))
            .Where(x => x.TargetUserId == targetUserId)
            .Where(x => x.StartTime <= now && x.EndTime >= now)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<IdentityUserDelegation> FindActiveDelegationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var now = Clock.Now;
        return await (await GetQueryableAsync(cancellationToken))
            .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.StartTime <= now &&
                    x.EndTime >= now
                , cancellationToken: GetCancellationToken(cancellationToken));
    }
}
