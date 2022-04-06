using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.OpenIddict.MongoDB;

namespace Volo.Abp.OpenIddict.Scopes;

public class MongoOpenIddictScopeRepository : MongoDbRepository<OpenIddictMongoDbContext, OpenIddictScope, Guid>, IOpenIddictScopeRepository
{
    public MongoOpenIddictScopeRepository(IMongoDbContextProvider<OpenIddictMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<OpenIddictScope> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken)).FirstOrDefaultAsync(x => x.Id == id, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictScope> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken)).FirstOrDefaultAsync(x => x.Name == name, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictScope>> FindByNamesAsync(string[] names, CancellationToken cancellationToken = default)
    {
        return await Queryable.Where((await GetMongoQueryableAsync(cancellationToken)), x => names.Contains(x.Name))
            .As<IMongoQueryable<OpenIddictScope>>()
            .ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictScope>> FindByResourceAsync(string resource, CancellationToken cancellationToken = default)
    {
        return await Queryable.Where((await GetMongoQueryableAsync(cancellationToken)), x => x.Resources.Contains(resource))
            .As<IMongoQueryable<OpenIddictScope>>()
            .ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictScope>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default)
    {
        return await Queryable.OrderBy((await GetMongoQueryableAsync(GetCancellationToken(cancellationToken))), x => x.Id)
            .SkipIf<OpenIddictScope, IQueryable<OpenIddictScope>>(offset.HasValue, offset.Value)
            .TakeIf<OpenIddictScope, IQueryable<OpenIddictScope>>(count.HasValue, count.Value)
            .As<IMongoQueryable<OpenIddictScope>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
