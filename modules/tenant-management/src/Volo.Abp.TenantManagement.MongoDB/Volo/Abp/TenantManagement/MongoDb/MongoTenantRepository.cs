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

namespace Volo.Abp.TenantManagement.MongoDb
{
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
            return await GetMongoQueryable()
                .FirstOrDefaultAsync(t => t.Name == name, cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().LongCountAsync(cancellationToken);
        }

        public virtual async Task<List<Tenant>> GetListAsync(
            string sorting = null, 
            int maxResultCount = int.MaxValue, 
            int skipCount = 0, 
            string filter = null, 
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .WhereIf<Tenant, IMongoQueryable<Tenant>>(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(filter)
                )
                .OrderBy(sorting ?? nameof(Tenant.Name))
                .As<IMongoQueryable<Tenant>>()
                .PageBy<Tenant, IMongoQueryable<Tenant>>(skipCount, maxResultCount)
                .ToListAsync(cancellationToken);
        }
    }
}