using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Linq.Dynamic.Core;
using MongoDB.Driver;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.TenantManagement.MongoDB;

public class MongoTenantRepository : MongoDbRepository<ITenantManagementMongoDbContext, Tenant, Guid>, ITenantRepository
{
    public MongoTenantRepository(IMongoDbContextProvider<ITenantManagementMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<Tenant> FindByNameAsync(
        string name,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .FirstOrDefaultAsync(t => t.Name == name, GetCancellationToken(cancellationToken));
    }

    [Obsolete("Use FindByNameAsync method.")]
    public virtual Tenant FindByName(string name, bool includeDetails = true)
    {
        return GetMongoQueryable()
            .FirstOrDefault(t => t.Name == name);
    }

    [Obsolete("Use FindAsync method.")]
    public virtual Tenant FindById(Guid id, bool includeDetails = true)
    {
        return GetMongoQueryable()
            .FirstOrDefault(t => t.Id == id);
    }

    public virtual async Task<List<Tenant>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf<Tenant, IMongoQueryable<Tenant>>(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.Name.Contains(filter)
            )
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(Tenant.Name) : sorting)
            .As<IMongoQueryable<Tenant>>()
            .PageBy<Tenant, IMongoQueryable<Tenant>>(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf<Tenant, IMongoQueryable<Tenant>>(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.Name.Contains(filter)
            ).CountAsync(cancellationToken: GetCancellationToken(cancellationToken));
    }
}
