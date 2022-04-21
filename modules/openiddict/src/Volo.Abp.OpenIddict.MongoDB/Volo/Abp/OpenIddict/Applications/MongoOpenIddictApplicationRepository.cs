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
using System.Linq.Dynamic.Core;

namespace Volo.Abp.OpenIddict.Applications;

public class MongoOpenIddictApplicationRepository : MongoDbRepository<OpenIddictMongoDbContext, OpenIddictApplication, Guid>, IOpenIddictApplicationRepository
{
    public MongoOpenIddictApplicationRepository(IMongoDbContextProvider<OpenIddictMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
    
    public async Task<List<OpenIddictApplication>> GetListAsync(string sorting, int skipCount, int maxResultCount, string filter = null,
        CancellationToken cancellationToken = default)
    {
        return await ((await GetMongoQueryableAsync(cancellationToken)))
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.ClientId.Contains(filter))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(OpenIddictApplication.ClientId) : sorting)
            .PageBy(skipCount, maxResultCount)
            .As<IMongoQueryable<OpenIddictApplication>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        return await ((await GetMongoQueryableAsync(cancellationToken)))
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.ClientId.Contains(filter))
            .As<IMongoQueryable<OpenIddictApplication>>()
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<OpenIddictApplication> FindByClientIdAsync(string clientId, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .FirstOrDefaultAsync(x => x.ClientId == clientId, cancellationToken);
    }

    public async Task<List<OpenIddictApplication>> FindByPostLogoutRedirectUriAsync(string address, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken)).Where(x => x.PostLogoutRedirectUris.Contains(address)).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<List<OpenIddictApplication>> FindByRedirectUriAsync(string address, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken)).Where(x => x.RedirectUris.Contains(address)).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken = default)
    {
        return await query(await GetMongoQueryableAsync(cancellationToken), state).As<IMongoQueryable<TResult>>().FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<List<OpenIddictApplication>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .SkipIf<OpenIddictApplication, IQueryable<OpenIddictApplication>>(offset.HasValue, offset)
            .TakeIf<OpenIddictApplication, IQueryable<OpenIddictApplication>>(count.HasValue, count)
            .As<IMongoQueryable<OpenIddictApplication>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
