using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.IdentityServer.ApiScopes;
using System.Linq.Dynamic.Core;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.MongoDB;

namespace Volo.Abp.IdentityServer.MongoDB
{
    public class MongoApiResourceRepository : MongoDbRepository<IAbpIdentityServerMongoDbContext, ApiResource, Guid>, IApiResourceRepository
    {
        public MongoApiResourceRepository(IMongoDbContextProvider<IAbpIdentityServerMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<ApiResource> FindByNameAsync(string apiResourceName, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(ar => ar.Name == apiResourceName)
                .OrderBy(ar => ar.Id)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<ApiResource>> FindByNameAsync(string[] apiResourceNames, bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(ar => apiResourceNames.Contains(ar.Name))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<ApiResource>> GetListByScopesAsync(string[] scopeNames, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(ar => ar.Scopes.Any(x => scopeNames.Contains(x.Scope)))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<ApiResource>> GetListAsync(string sorting, int skipCount, int maxResultCount, string filter, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .WhereIf(!filter.IsNullOrWhiteSpace(),
                         x => x.Name.Contains(filter) ||
                         x.Description.Contains(filter) ||
                         x.DisplayName.Contains(filter))
                .OrderBy(sorting ?? nameof(ApiResource.Name))
                .As<IMongoQueryable<ApiResource>>()
                .PageBy<ApiResource, IMongoQueryable<ApiResource>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<long> GetTotalCount()
        {
            return await GetCountAsync();
        }

        public virtual async Task<bool> CheckNameExistAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().AnyAsync(ar => ar.Id != expectedId && ar.Name == name, GetCancellationToken(cancellationToken));
        }
    }
}
