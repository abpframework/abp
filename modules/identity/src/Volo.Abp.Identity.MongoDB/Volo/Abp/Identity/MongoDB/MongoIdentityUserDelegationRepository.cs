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
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf(sourceUserId.HasValue, x => x.SourceUserId == sourceUserId)
            .WhereIf(targetUserId.HasValue, x => x.TargetUserId == targetUserId)
            .As<IMongoQueryable<IdentityUserDelegation>>()
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<List<IdentityUserDelegation>> GetActiveDelegationsAsync(Guid targetUserId, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(x => x.TargetUserId == targetUserId)
            .Where(x => x.StartTime <= Clock.Now && x.EndTime >= Clock.Now)
            .As<IMongoQueryable<IdentityUserDelegation>>()
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<IdentityUserDelegation> FindActiveDelegationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.StartTime <= Clock.Now &&
                    x.EndTime >= Clock.Now
                , cancellationToken: GetCancellationToken(cancellationToken));
    }
}
