using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.MongoDB;
using System.Linq.Dynamic.Core;

namespace Volo.Abp.IdentityServer.MongoDB
{
    public class MongoIdentityResourceRepository : MongoDbRepository<IAbpIdentityServerMongoDbContext, IdentityResource, Guid>, IIdentityResourceRepository
    {
        public MongoIdentityResourceRepository(IMongoDbContextProvider<IAbpIdentityServerMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<List<IdentityResource>> GetListAsync(string sorting, int skipCount, int maxResultCount, string filter, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter) ||
                         x.Description.Contains(filter) ||
                         x.DisplayName.Contains(filter))
                .OrderBy(sorting ?? nameof(IdentityResource.Name))
                .As<IMongoQueryable<IdentityResource>>()
                .PageBy<IdentityResource, IMongoQueryable<IdentityResource>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<IdentityResource> FindByNameAsync(
            string name,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(x => x.Name == name)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityResource>> GetListByScopeNameAsync(string[] scopeNames, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(ar => scopeNames.Contains(ar.Name))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<long> GetTotalCountAsync()
        {
            return await GetCountAsync();
        }

        public virtual async Task<bool> CheckNameExistAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().AnyAsync(ir => ir.Id != expectedId && ir.Name == name, cancellationToken: cancellationToken);
        }
    }
}
