using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;

namespace Volo.Abp.OpenIddict.Applications;

public class EfCoreOpenIddictApplicationRepository : EfCoreRepository<IOpenIddictDbContext, OpenIddictApplication, Guid>, IOpenIddictApplicationRepository
{
    public EfCoreOpenIddictApplicationRepository(IDbContextProvider<IOpenIddictDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<long> CountAsync<TResult>(Func<IQueryable<OpenIddictApplication>, IQueryable<TResult>> query, CancellationToken cancellationToken = default)
    {
        return await query(await GetDbSetAsync()).LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictApplication> FindByClientIdAsync(string clientId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .FirstOrDefaultAsync(x => x.ClientId == clientId, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictApplication>>  FindByPostLogoutRedirectUriAsync(string address, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.PostLogoutRedirectUris.Contains(address)).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictApplication>>  FindByRedirectUriAsync(string address, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.RedirectUris.Contains(address)).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken = default)
    {
        return await query(await GetDbSetAsync(), state)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictApplication>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .SkipIf<OpenIddictApplication, IQueryable<OpenIddictApplication>>(offset.HasValue, offset.Value)
            .TakeIf<OpenIddictApplication, IQueryable<OpenIddictApplication>>(count.HasValue, count.Value)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<TResult>> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken = default)
    {
        return await query(await GetDbSetAsync(), state)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
