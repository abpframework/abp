using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB;

public class MongoIdentityRoleRepository : MongoDbRepository<IAbpIdentityMongoDbContext, IdentityRole, Guid>, IIdentityRoleRepository
{
    public MongoIdentityRoleRepository(IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<IdentityRole> FindByNormalizedNameAsync(
        string normalizedRoleName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityRole>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                    x => x.Name.Contains(filter) ||
                    x.NormalizedName.Contains(filter))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(IdentityRole.Name) : sorting)
            .As<IMongoQueryable<IdentityRole>>()
            .PageBy<IdentityRole, IMongoQueryable<IdentityRole>>(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityRole>> GetListAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(t => ids.Contains(t.Id))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityRole>> GetDefaultOnesAsync(
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(r => r.IsDefault)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetCountAsync(
        string filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.Name.Contains(filter) ||
                     x.NormalizedName.Contains(filter))
            .As<IMongoQueryable<IdentityRole>>()
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }
}
