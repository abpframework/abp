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
using Volo.Abp.MongoDB;

namespace Volo.Abp.IdentityServer.MongoDB
{
    public class MongoApiScopeRepository : MongoDbRepository<IAbpIdentityServerMongoDbContext, ApiScope, Guid>,
        IApiScopeRepository
    {
        public MongoApiScopeRepository(IMongoDbContextProvider<IAbpIdentityServerMongoDbContext> dbContextProvider) :
            base(dbContextProvider)
        {
        }

        public async Task<ApiScope> FindByNameAsync(string scopeName, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(x => x.Name == scopeName, GetCancellationToken(cancellationToken));
        }

        public async Task<List<ApiScope>> GetListByNameAsync(string[] scopeNames, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .Where(scope => scopeNames.Contains(scope.Name))
                .OrderBy(scope => scope.Id)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<ApiScope>> GetListAsync(string sorting, int skipCount, int maxResultCount, string filter = null, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .WhereIf(!filter.IsNullOrWhiteSpace(),
                    x => x.Name.Contains(filter) ||
                         x.Description.Contains(filter) ||
                         x.DisplayName.Contains(filter))
                .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(ApiScope.Name) : sorting)
                .As<IMongoQueryable<ApiScope>>()
                .PageBy<ApiScope, IMongoQueryable<ApiScope>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .WhereIf<ApiScope, IMongoQueryable<ApiScope>>(!filter.IsNullOrWhiteSpace(),
                    x => x.Name.Contains(filter) ||
                         x.Description.Contains(filter) ||
                         x.DisplayName.Contains(filter))
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<bool> CheckNameExistAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .AnyAsync(x => x.Id != expectedId && x.Name == name, GetCancellationToken(cancellationToken));
        }
    }
}
