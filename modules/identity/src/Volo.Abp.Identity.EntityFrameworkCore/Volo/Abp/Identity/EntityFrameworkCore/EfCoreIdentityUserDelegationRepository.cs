using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Timing;

namespace Volo.Abp.Identity.EntityFrameworkCore;

public class EfCoreIdentityUserDelegationRepository : EfCoreRepository<IIdentityDbContext, IdentityUserDelegation, Guid>, IIdentityUserDelegationRepository
{
    protected IClock Clock { get; }
    
    public EfCoreIdentityUserDelegationRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider, IClock clock)
        : base(dbContextProvider)
    {
        Clock = clock;
    }

    public virtual async Task<List<IdentityUserDelegation>> GetListAsync(Guid? sourceUserId, Guid? targetUserId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .WhereIf(sourceUserId.HasValue, x => x.SourceUserId == sourceUserId)
            .WhereIf(targetUserId.HasValue, x => x.TargetUserId == targetUserId)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<List<IdentityUserDelegation>> GetActiveDelegationsAsync(Guid targetUserId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Where(x => x.TargetUserId == targetUserId && 
                        x.StartTime <= Clock.Now && 
                        x.EndTime >= Clock.Now)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<IdentityUserDelegation> FindActiveDelegationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.StartTime <= Clock.Now &&
                    x.EndTime >= Clock.Now
                , cancellationToken: GetCancellationToken(cancellationToken));
    }
}
