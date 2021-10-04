using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
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
            return await (await GetMongoQueryableAsync(cancellationToken))
                .OrderBy(ar => ar.Id)
                .FirstOrDefaultAsync(ar => ar.Name == apiResourceName, GetCancellationToken(cancellationToken));
        }

        public async Task<List<ApiResource>> FindByNameAsync(string[] apiResourceNames, bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .Where(ar => apiResourceNames.Contains(ar.Name))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<ApiResource>> GetListByScopesAsync(string[] scopeNames, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .Where(ar => ar.Scopes.Any(x => scopeNames.Contains(x.Scope)))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<ApiResource>> GetListAsync(string sorting, int skipCount, int maxResultCount, string filter, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .WhereIf(!filter.IsNullOrWhiteSpace(),
                         x => x.Name.Contains(filter) ||
                         x.Description.Contains(filter) ||
                         x.DisplayName.Contains(filter))
                .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(ApiResource.Name) : sorting)
                .As<IMongoQueryable<ApiResource>>()
                .PageBy<ApiResource, IMongoQueryable<ApiResource>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .WhereIf<ApiResource, IMongoQueryable<ApiResource>>(!filter.IsNullOrWhiteSpace(),
                x => x.Name.Contains(filter) ||
                     x.Description.Contains(filter) ||
                     x.DisplayName.Contains(filter))
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> CheckNameExistAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .AnyAsync(ar => ar.Id != expectedId && ar.Name == name, GetCancellationToken(cancellationToken));
        }
    }
}
