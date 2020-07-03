using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.IdentityServer.ApiScopes;
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

        public async Task<List<ApiScope>> GetListByNameAsync(string[] scopeNames, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from scope in GetMongoQueryable()
                where scopeNames.Contains(scope.Name)
                select scope;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
