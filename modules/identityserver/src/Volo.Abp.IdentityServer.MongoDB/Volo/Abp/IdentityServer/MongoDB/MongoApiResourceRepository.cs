using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.MongoDB;

namespace Volo.Abp.IdentityServer.MongoDB
{
    public class MongoApiResourceRepository : MongoDbRepository<IAbpIdentityServerMongoDbContext, ApiResource, Guid>, IApiResourceRepository
    {
        public MongoApiResourceRepository(IMongoDbContextProvider<IAbpIdentityServerMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<ApiResource> FindByNameAsync(string name, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(ar=>ar.Name == name)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<ApiResource>> GetListByScopesAsync(string[] scopeNames, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(ar=>ar.Scopes.Any(x=> scopeNames.Contains(x.Name)))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
