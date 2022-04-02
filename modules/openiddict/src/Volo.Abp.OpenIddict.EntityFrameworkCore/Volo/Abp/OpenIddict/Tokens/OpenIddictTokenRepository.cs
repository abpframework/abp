using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.EntityFrameworkCore;

namespace Volo.Abp.OpenIddict.Tokens;

public class OpenIddictTokenRepository : EfCoreRepository<IOpenIddictDbContext, OpenIddictToken, Guid>, IOpenIddictTokenRepository
{
    public OpenIddictTokenRepository(IDbContextProvider<IOpenIddictDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<long> CountAsync<TResult>(Func<IQueryable<OpenIddictToken>, IQueryable<TResult>> query, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => x.Subject == subject && x.ApplicationId == client).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, string status, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => x.Subject == subject && x.ApplicationId == client && x.Status == status).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, string status, string type, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => x.Subject == subject && x.ApplicationId == client && x.Status == status && x.Type == type).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<OpenIddictToken>> FindByApplicationIdAsync(Guid applicationId, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => x.ApplicationId == applicationId).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<OpenIddictToken>> FindByAuthorizationIdAsync(Guid authorizationId, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => x.AuthorizationId == authorizationId).ToListAsync(cancellationToken);
    }

    public virtual async Task<OpenIddictToken> FindByIdAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public virtual async Task<OpenIddictToken> FindByReferenceIdAsync(string referenceId, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).FirstOrDefaultAsync(x => x.ReferenceId == referenceId, cancellationToken);
    }

    public virtual async Task<List<OpenIddictToken>> FindBySubjectAsync(string subject, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => x.Subject == subject).ToListAsync(cancellationToken);
    }

    public virtual async Task<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictToken>, TState, IQueryable<TResult>> query, TState state, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await query(await GetQueryableAsync(), state).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<List<OpenIddictToken>> ListAsync(int? count, int? offset, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .OrderBy(x => x.Id)
            .SkipIf<OpenIddictToken, IQueryable<OpenIddictToken>>(offset.HasValue, offset.Value)
            .TakeIf<OpenIddictToken, IQueryable<OpenIddictToken>>(count.HasValue, count.Value)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TResult>> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictToken>, TState, IQueryable<TResult>> query, TState state, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await query(await GetQueryableAsync(), state).ToListAsync(cancellationToken);
    }

    public async Task<List<OpenIddictToken>> GetPruneListAsync(DateTime date, int count, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        //TODO: Test & Improve?
        return await (from token in await GetQueryableAsync()
            join authorization in (await GetDbContextAsync()).Set<OpenIddictAuthorization>().AsQueryable()
                on token.AuthorizationId equals authorization.Id into ta
            from a in ta
            where token.CreationDate < date
            where (token.Status != OpenIddictConstants.Statuses.Inactive &&
                   token.Status != OpenIddictConstants.Statuses.Valid) ||
                  (a != null && a.Status != OpenIddictConstants.Statuses.Valid) ||
                  token.ExpirationDate < DateTime.UtcNow
            orderby token.Id
            select token).Take(count).ToListAsync(cancellationToken);
    }
}
