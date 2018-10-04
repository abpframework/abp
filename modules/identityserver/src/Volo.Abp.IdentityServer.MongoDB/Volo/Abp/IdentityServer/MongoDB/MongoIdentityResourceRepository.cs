using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.MongoDB;

namespace Volo.Abp.IdentityServer.MongoDB
{
    public class MongoIdentityResourceRepository : MongoDbRepository<IAbpIdentityServerMongoDbContext, IdentityResource, Guid>, IIdentityResourceRepository
    {
        public MongoIdentityResourceRepository(IMongoDbContextProvider<IAbpIdentityServerMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<IdentityResource>> GetListByScopesAsync(string[] scopeNames, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(ar => scopeNames.Any(s=>s == ar.Name))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
