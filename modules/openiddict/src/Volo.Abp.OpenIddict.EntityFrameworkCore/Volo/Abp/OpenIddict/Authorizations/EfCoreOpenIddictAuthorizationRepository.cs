using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;

namespace Volo.Abp.OpenIddict.Authorizations;

public class EfCoreOpenIddictAuthorizationRepository : EfCoreRepository<IOpenIddictDbContext, OpenIddictAuthorization, Guid>, IOpenIddictAuthorizationRepository
{
    public EfCoreOpenIddictAuthorizationRepository(IDbContextProvider<IOpenIddictDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<long> CountAsync<TResult>(Func<IQueryable<OpenIddictAuthorization>, IQueryable<TResult>> query, CancellationToken cancellationToken = default)
    {
        return await query(await GetQueryableAsync()).LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.Subject == subject && x.ApplicationId == client)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, string status, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.Subject == subject && x.Status == status && x.ApplicationId == client)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, string status, string type, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.Subject == subject && x.Status == status && x.Type == type && x.ApplicationId == client)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.ApplicationId == applicationId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictAuthorization> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .FirstOrDefaultAsync(x => x.Id == id, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindBySubjectAsync(string subject, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.Subject == subject)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictAuthorization>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken = default)
    {
        return await query(await GetDbSetAsync(), state)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default)
    {
        var query = (await GetDbSetAsync())
            .OrderBy(authorization => authorization.Id!)
            .AsTracking();

        if (offset.HasValue)
        {
            query = query.Skip(offset.Value);
        }

        if (count.HasValue)
        {
            query = query.Take(count.Value);
        }

        return await query.ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<TResult>> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictAuthorization>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken = default)
    {
        return await query(await GetDbSetAsync(), state)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> GetPruneListAsync(DateTime date, int count, CancellationToken cancellationToken = default)
    {
        var tokenQueryable = (await GetDbContextAsync()).Tokens.AsQueryable();
        return await (await GetDbSetAsync())
            .Where(x => x.CreationDate < date)
            .Where(x => x.Status != OpenIddictConstants.Statuses.Valid ||
                        (x.Type == OpenIddictConstants.AuthorizationTypes.AdHoc && tokenQueryable.Any(t => t.AuthorizationId == x.Id)))
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
