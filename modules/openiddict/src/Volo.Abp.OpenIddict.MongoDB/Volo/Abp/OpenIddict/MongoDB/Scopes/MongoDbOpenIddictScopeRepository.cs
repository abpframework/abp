using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.OpenIddict.MongoDB;

namespace Volo.Abp.OpenIddict.Scopes
{
    public class MongoDbOpenIddictScopeRepository
        : MongoDbRepository<IOpenIddictMongoDbContext, OpenIddictScope, Guid>, IOpenIddictScopeRepository
    {
        public MongoDbOpenIddictScopeRepository(IMongoDbContextProvider<IOpenIddictMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<OpenIddictScope> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return await query.FirstOrDefaultAsync(a => a.Name == name, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictScope>> FindByNamesAsync(
            List<string> names,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            return await query.Where(x => names.Contains(x.Name)).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictScope>> FindByResourceAsync(
            string resource,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            query = query
                .Where(x => x.Resources.Contains(resource));

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OpenIddictScope>> GetListAsync(
            int? maxResultCount,
            int? skipCount,
            CancellationToken cancellationToken = default)
        {
            var query = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));

            if (skipCount.HasValue)
            {
                query = query.Skip(skipCount.Value);
            }
            if (maxResultCount.HasValue)
            {
                query = query.Take(maxResultCount.Value);
            }
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}