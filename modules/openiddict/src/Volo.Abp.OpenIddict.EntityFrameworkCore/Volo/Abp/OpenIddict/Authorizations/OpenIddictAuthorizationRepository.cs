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

public class OpenIddictAuthorizationRepository : EfCoreRepository<IOpenIddictDbContext, OpenIddictAuthorization, Guid>, IOpenIddictAuthorizationRepository
{
    public OpenIddictAuthorizationRepository(IDbContextProvider<IOpenIddictDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<long> CountAsync<TResult>(Func<IQueryable<OpenIddictAuthorization>, IQueryable<TResult>> query, CancellationToken cancellationToken = default)
    {
        return await query(await GetQueryableAsync()).LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(x => x.Subject == subject && x.ApplicationId == client)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, string status, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(x => x.Subject == subject && x.Status == status && x.ApplicationId == client)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, string status, string type, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(x => x.Subject == subject && x.Status == status && x.Type == type && x.ApplicationId == client)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindByApplicationIdAsync(Guid applicationId, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(x => x.ApplicationId == applicationId)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<OpenIddictAuthorization> FindByIdAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindBySubjectAsync(string subject, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(x => x.Subject == subject)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictAuthorization>, TState, IQueryable<TResult>> query, TState state, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await query((await GetDbSetAsync())
            .IncludeDetails(includeDetails), state)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<List<OpenIddictAuthorization>> ListAsync(int? count, int? offset, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var query = (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
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

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TResult>> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictAuthorization>, TState, IQueryable<TResult>> query, TState state, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await query((await GetDbSetAsync())
            .IncludeDetails(includeDetails), state)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<OpenIddictAuthorization>> GetPruneListAsync(DateTime date, int count, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(x => x.CreationDate < date)
            .Where(x => x.Status != OpenIddictConstants.Statuses.Valid || (x.Type == OpenIddictConstants.AuthorizationTypes.AdHoc && !x.Tokens.Any()))
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    [Obsolete("Use WithDetailsAsync method.")]
    public override IQueryable<OpenIddictAuthorization> WithDetails()
    {
        return GetQueryable().IncludeDetails();
    }

    public async override Task<IQueryable<OpenIddictAuthorization>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}
