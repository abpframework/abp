using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Identity.EntityFrameworkCore;

public class EfCoreIdentityUserDelegationRepository : EfCoreRepository<IIdentityDbContext, IdentityUserDelegation, Guid>, IIdentityUserDelegationRepository
{
    public EfCoreIdentityUserDelegationRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<IdentityUserDelegation>> GetListAsync(Guid? sourceUserId, Guid? targetUserId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .WhereIf(sourceUserId.HasValue, x => x.SourceUserId == sourceUserId)
            .WhereIf(targetUserId.HasValue, x => x.TargetUserId == targetUserId)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<IdentityUserDelegation> FindAsync(Guid sourceUserId, Guid targetUserId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                    x.SourceUserId == sourceUserId &&
                    x.TargetUserId == targetUserId
                , cancellationToken: GetCancellationToken(cancellationToken));
    }
}
