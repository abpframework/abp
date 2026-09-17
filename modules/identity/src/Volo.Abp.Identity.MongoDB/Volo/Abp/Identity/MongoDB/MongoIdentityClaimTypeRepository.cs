using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB;

public class MongoIdentityClaimTypeRepository : MongoDbRepository<IAbpIdentityMongoDbContext, IdentityClaimType, Guid>, IIdentityClaimTypeRepository
{
    public MongoIdentityClaimTypeRepository(IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<bool> AnyAsync(
        string name,
        Guid? ignoredId = null,
        CancellationToken cancellationToken = default)
    {
        if (ignoredId == null)
        {
            return await (await GetQueryableAsync(cancellationToken))
                .Where(ct => ct.Name == name)
                .AnyAsync(GetCancellationToken(cancellationToken));
        }
        else
        {
            return await (await GetQueryableAsync(cancellationToken))
                .Where(ct => ct.Id != ignoredId && ct.Name == name)
                .AnyAsync(GetCancellationToken(cancellationToken));
        }
    }

    public virtual async Task<List<IdentityClaimType>> GetListAsync(
        string sorting,
        int maxResultCount,
        int skipCount,
        string filter,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.Name.Contains(filter)
            )
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(IdentityClaimType.CreationTime) + " desc" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetCountAsync(
        string filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.Name.Contains(filter)
            )
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityClaimType>> GetListByNamesAsync(IEnumerable<string> names, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .Where(x => names.Contains(x.Name))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
