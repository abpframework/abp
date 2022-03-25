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

public class OpenIddictApplicationRepository : EfCoreRepository<IOpenIddictDbContext, OpenIddictApplication, Guid>, IOpenIddictApplicationRepository
{
    public OpenIddictApplicationRepository(IDbContextProvider<IOpenIddictDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<long> CountAsync<TResult>(Func<IQueryable<OpenIddictApplication>, IQueryable<TResult>> query, CancellationToken cancellationToken)
    {
        return await query(await GetQueryableAsync()).LongCountAsync(cancellationToken);
    }

    public virtual async Task<OpenIddictApplication> FindByClientIdAsync(string clientId, CancellationToken cancellationToken)
    {
        return await (await GetQueryableAsync()).FirstOrDefaultAsync(x => x.ClientId == clientId, cancellationToken);
    }

    public virtual async Task<List<OpenIddictApplication>>  FindByPostLogoutRedirectUriAsync(string address, CancellationToken cancellationToken)
    {
        return await (await GetQueryableAsync()).Where(x => x.PostLogoutRedirectUris.Contains(address)).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<OpenIddictApplication>>  FindByRedirectUriAsync(string address, CancellationToken cancellationToken)
    {
        return await (await GetQueryableAsync()).Where(x => x.RedirectUris.Contains(address)).ToListAsync(cancellationToken);
    }

    public virtual async Task<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        return await query(await GetQueryableAsync(), state).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<List<OpenIddictApplication>> ListAsync(int? count, int? offset, CancellationToken cancellationToken)
    {
        return await (await GetQueryableAsync())
            .OrderBy(x => x.Id)
            .SkipIf<OpenIddictApplication, IQueryable<OpenIddictApplication>>(offset.HasValue, offset.Value)
            .TakeIf<OpenIddictApplication, IQueryable<OpenIddictApplication>>(count.HasValue, count.Value)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TResult>> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        return await query(await GetQueryableAsync(), state).ToListAsync(cancellationToken);
    }

    [Obsolete("Use WithDetailsAsync method.")]
    public override IQueryable<OpenIddictApplication> WithDetails()
    {
        return GetQueryable().IncludeDetails();
    }

    public async override Task<IQueryable<OpenIddictApplication>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}
