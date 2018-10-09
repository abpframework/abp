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

        public virtual async Task<List<IdentityResource>> GetListAsync(string sorting, int skipCount, int maxResultCount, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .OrderBy(sorting ?? nameof(IdentityResource.Name))
                .As<IMongoQueryable<IdentityResource>>()
                .PageBy<IdentityResource, IMongoQueryable<IdentityResource>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<IdentityResource>> GetListByScopesAsync(string[] scopeNames, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(ar => scopeNames.Contains(ar.Name))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<long> GetTotalBlogCount()
        {
            return await GetCountAsync();
        }
    }
}
